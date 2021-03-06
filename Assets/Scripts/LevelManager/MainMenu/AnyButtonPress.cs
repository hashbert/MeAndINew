using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using FMODUnity;

public class AnyButtonPress : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject backSelectPanel;

    private readonly InputAction _anyKeyWait = new InputAction(binding: "/*/<button>", type: InputActionType.Button);
    private void Awake() => _anyKeyWait.performed += DoSomething;
    private void OnEnable() => _anyKeyWait.Enable();
    private void OnDisable() => _anyKeyWait.Disable();
    private void OnDestroy() => _anyKeyWait.performed -= DoSomething;
    private void DoSomething(InputAction.CallbackContext ctx) => AnyKey();
    private void AnyKey()
    {
        startScreen.SetActive(false);
        startMenu.SetActive(true);
        backSelectPanel.SetActive(true);
        RuntimeManager.PlayOneShotAttached("event:/SFX/UI/Game Start", gameObject);
    }

}
