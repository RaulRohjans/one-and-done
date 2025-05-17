using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacterScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public SpriteRenderer spriteRenderer;
    public Sprite idleSprite, jumpSprite;

    public float groundCheckRadius = 0.1f;
    enum PlayerState { Idle, Jumping, Falling }
    PlayerState state = PlayerState.Idle;
    Rigidbody2D.SlideMovement SlideMovement = new Rigidbody2D.SlideMovement();


    public PlayerInputActions controls;
    private InputAction move, jump;
    Vector2 moveDirection = Vector2.zero;

    float maxMovSpeed = 15f;
    float jumpForce = 13f;

    private void Awake()
    {
        controls = new PlayerInputActions();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocity.y > 0.1f && !isGrounded() && state != PlayerState.Jumping)
        {
            // JUMPING
            state = PlayerState.Jumping;
            spriteRenderer.sprite = jumpSprite;
        }
        else if (rb.linearVelocity.y < -0.1f && !isGrounded() && state != PlayerState.Falling)
        {
            // FALLING
            state = PlayerState.Falling;
        }
        else if (isGrounded() && state != PlayerState.Idle)
        {
            // IDLE
            state = PlayerState.Idle;
            spriteRenderer.sprite = idleSprite;
        }

        moveDirection = move.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        // Rotate bunny
        if (moveDirection.x < -0.01f) spriteRenderer.flipX = true; // left
        else if (moveDirection.x > 0.01f) spriteRenderer.flipX = false; // right (default)

        rb.AddForce(new Vector2(moveDirection.x * 60, 0f));

        // Clamp horizontal speed
        if (Mathf.Abs(rb.linearVelocity.x) > maxMovSpeed)
            rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * maxMovSpeed, rb.linearVelocity.y);        
    }


    private void OnEnable()
    {
        jump = controls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;

        move = controls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        jump.Disable();
        move.Disable();
    }   

    private void Jump(InputAction.CallbackContext context) {
        if(!isGrounded()) return;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool isGrounded() {
        Vector2 boxCenter = groundCheck.position;
        Vector2 boxSize = new Vector2(0.8776389f, 0.07355173f);

        return Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundLayer);
    }
}
