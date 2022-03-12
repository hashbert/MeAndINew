using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCharacter : MonoBehaviour
{
    public bool KidActive { get; private set; }

    //kid
    private GameObject kid;
    private Rigidbody2D kidRb;
    private Kid kidScript;
    private BoxCollider2D kidBoxColl;
    private Animator kidAnim;
    private PlayerColorSwap kidColorSwap;

    //adult
    private GameObject adult;
    private Rigidbody2D adultRb;
    private Adult adultScript;
    private BoxCollider2D adultBoxColl;
    private Animator adultAnim;
    private PlayerColorSwap adultColorSwap;

    //camera background
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Color initialBackgroundColor;
    [SerializeField] private Color invertedBackgroundColor;

    private void Awake()
    {
        //kid
        kid = GameObject.Find("Kid");
        kidRb = kid.GetComponent<Rigidbody2D>();
        kidScript = kid.GetComponent<Kid>();
        kidBoxColl = kid.GetComponent<BoxCollider2D>();
        kidAnim = kid.GetComponent<Animator>();
        KidActive = true;
        kidColorSwap = kid.GetComponent<PlayerColorSwap>();

        //adult
        adult = GameObject.Find("Adult");
        adultRb = adult.GetComponent<Rigidbody2D>();
        adultScript = adult.GetComponent<Adult>();
        adultBoxColl = adult.GetComponent<BoxCollider2D>();
        adultAnim = adult.GetComponent<Animator>();
        adultColorSwap = adult.GetComponent<PlayerColorSwap>();

        //camera
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        initialBackgroundColor = mainCamera.backgroundColor;
        invertedBackgroundColor = new Color(1f - initialBackgroundColor.r, 1f - initialBackgroundColor.g, 1f - initialBackgroundColor.b);

    }
    // Start is called before the first frame update
    void Start()
    {
        FreezeAdult();
    }

    private void FreezeAdult()
    {
        //freeze adult and allow walk through
        adultRb.bodyType = RigidbodyType2D.Static;
        adultScript.enabled = false;
        adultBoxColl.enabled = false;
        adultAnim.enabled = false;
        InputManager.playerInput.actions["Grab"].Disable();
        adultColorSwap.Swap();
    }
    private void UnfreezeAdult()
    {
        //unfreeze adult and allow walk through
        adultRb.bodyType = RigidbodyType2D.Dynamic;
        adultScript.enabled = true;
        adultBoxColl.enabled = true;
        adultAnim.enabled = true;
        InputManager.playerInput.actions["Grab"].Enable();
        adultColorSwap.ResetSwap();
    }
    private void FreezeKid()
    {
        //freeze kid and allow walk through
        kidRb.bodyType = RigidbodyType2D.Static;
        kidScript.enabled = false;
        kidBoxColl.enabled = false;
        kidAnim.enabled = false;
        kidColorSwap.Swap();

    }
    private void UnfreezeKid()
    {
        //unfreeze kid and allow walk through
        kidRb.bodyType = RigidbodyType2D.Dynamic;
        kidScript.enabled = true;
        kidBoxColl.enabled = true;
        kidAnim.enabled = true;
        kidColorSwap.ResetSwap();
    }


    private void SwapColor()
    {
        if (mainCamera.backgroundColor == initialBackgroundColor)
        {
            mainCamera.backgroundColor = invertedBackgroundColor;
        }
        else
        {
            mainCamera.backgroundColor = initialBackgroundColor;
        }
    }

    public void OnSwitchCharacter(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (KidActive && kidAnim.GetInteger("KidState") == 0 && kidScript.IsOnGround())
            {
                FreezeKid();
                UnfreezeAdult();
                SwapColor();
                KidActive = !KidActive;
            }
            else if (!KidActive)
            {
                UnfreezeKid();
                FreezeAdult();
                SwapColor();
                KidActive = !KidActive;
            }
        }
    }
}
