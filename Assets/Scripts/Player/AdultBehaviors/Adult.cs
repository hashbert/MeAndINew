using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Adult : MonoBehaviour
{
    #region Awake() variables
    public Rigidbody2D PlayerRb { get; private set; }
    public Collider2D PlayerColl { get; private set; }
    #endregion

    #region Other Variables
    
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float horizontalInput;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float slowdownFraction = .5f;
    
    [SerializeField] private InputActionReference jump;
    public bool IsFacingRight { get; private set; }
    //[SerializeField] private InputActionReference grab;

    private float boxExtensionHeight = 0.1f; //raycast for checking if grounded
    
    #endregion

    #region Unity Callback Functions

    private void Awake()
    {
        IsFacingRight = true;
        PlayerRb = GetComponent<Rigidbody2D>();
        PlayerColl = GetComponent<Collider2D>();
        //grab.action.Disable();
    }
    private void Update()
    {
        Color color = Color.red;
        Debug.DrawRay(PlayerColl.bounds.center, Vector2.down * (PlayerColl.bounds.extents.y + boxExtensionHeight), color);
        Move();
    }

    private void OnEnable()
    {
        jump.action.started += OnJumpInput;
        jump.action.canceled += OnJumpInput;
    }

    private void OnDisable()
    {
        jump.action.started -= OnJumpInput;
        jump.action.canceled -= OnJumpInput;
    }

    #endregion

    #region Player Input Abilities
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started && IsGrounded())
        {
            PlayerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        if (context.canceled && PlayerRb.velocity.y > 0f)
        {
            PlayerRb.AddForce(new Vector2(0f, -PlayerRb.velocity.y * slowdownFraction), ForceMode2D.Impulse);
        }
    }
    #endregion

    #region Check Functions
    public bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(PlayerColl.bounds.center, PlayerColl.bounds.size, 0f, Vector2.down, boxExtensionHeight, groundLayerMask);
        return raycastHit.collider != null;
    }
    private void CheckIfShouldFlip()
    {
        if (IsFacingRight && horizontalInput < 0f)
        {
            Flip();
        }
        else if (!IsFacingRight && horizontalInput > 0f)
        {
            Flip();
        }
    }
    #endregion

    #region Other Functions
    private void Move()
    {
        PlayerRb.velocity = new Vector2(horizontalInput * speed, PlayerRb.velocity.y);
        CheckIfShouldFlip();
    }

    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    #endregion
}
