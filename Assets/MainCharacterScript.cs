using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacterScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    // Player sprites
    public SpriteRenderer spriteRenderer;
    public Sprite idleSprite, jumpSprite;

    // Play state
    enum PlayerState { Idle, Jumping, Falling }
    PlayerState state = PlayerState.Idle;
    Rigidbody2D.SlideMovement SlideMovement = new Rigidbody2D.SlideMovement();

    // Controls variables
    public PlayerInputActions controls;
    private InputAction move, jump;
    Vector2 moveDirection = Vector2.zero;

    // Movement variables
    float maxMovSpeed = 15f;
    float jumpForce = 13f;

    // Power-ups
    private bool hasShield = false;
    private bool canDoubleJump = false;
    public Sprite shieldIcon, doubleJumpIcon;
    public UnityEngine.UI.Image activeEffectIcon;

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
        if (rb.linearVelocity.y > 0.1f && !IsGrounded() && state != PlayerState.Jumping)
        {
            // JUMPING
            state = PlayerState.Jumping;
            spriteRenderer.sprite = jumpSprite;
        }
        else if (rb.linearVelocity.y < -0.1f && !IsGrounded() && state != PlayerState.Falling)
        {
            // FALLING
            state = PlayerState.Falling;
        }
        else if (IsGrounded() && state != PlayerState.Idle)
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

    private void Jump(InputAction.CallbackContext context)
    {
        if (!IsGrounded() && !canDoubleJump) return;
        else if(!IsGrounded()) ClearPickups(); // Only clear power up if actually used

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool IsGrounded()
    {
        Vector2 boxCenter = groundCheck.position;
        Vector2 boxSize = new Vector2(0.8776389f, 0.07355173f);

        return Physics2D.OverlapBox(boxCenter, boxSize, 0f, groundLayer);
    }

    private void ClearPickups()
    {
        hasShield = false;
        canDoubleJump = false;

        ClearIcon();
    }

    void SetIcon(Sprite icon)
    {
        activeEffectIcon.sprite = icon;

        // The invisibility of the image is dictated by the a value
        // of the color, by setting it to 1 the image will be shown
        Color c = activeEffectIcon.color;
        c.a = 1f;
        activeEffectIcon.color = c;
    }

    void ClearIcon()
    {
        activeEffectIcon.sprite = null;

        // Here we set the value to 0 to preserve the color 
        // and only make it invisible
        Color c = activeEffectIcon.color;
        c.a = 0f;
        activeEffectIcon.color = c;
    }

    public void Pickup(Drop drop)
    {
        switch (drop.type)
        {
            case DropType.Carrot:
                // Increase score (you can add a score system)
                break;

            case DropType.EvilCarrot:
                if (hasShield) ClearPickups();
                else
                {
                    // Game over logic here
                    Debug.Log("You died!");
                }
                break;

            case DropType.Shield:
                ClearPickups();

                hasShield = true;
                SetIcon(shieldIcon);
                break;

            case DropType.DoubleJump:
                ClearPickups();

                canDoubleJump = true;
                SetIcon(doubleJumpIcon);
                break;
        }
    }
}
