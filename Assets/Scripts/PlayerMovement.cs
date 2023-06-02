using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;
    private bool isWallSliding = false;
    private bool isWallJumping = false;
    private bool isJumping = false;
    private bool canDoubleJump = false;

    public float wallJumpForce = 10f;
    public float wallSlideSpeed = 2f;
    public float moveSpeed = 7f;
    public float jumpForce = 10f;

    private float dirX = 0f;

    private enum MovementState { idle, running, jumping }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        Application.targetFrameRate = 30;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isWallSliding && !isWallJumping)
        {
            DoWallJump();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dirX = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dirX = 1f;
        }
        else
        {
            dirX = 0f;
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (IsGrounded())
            {
                Jump();
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                Jump();
                canDoubleJump = false;
            }
            else if (isWallSliding && !isWallJumping)
            {
                DoWallJump();
            }
        }

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        UpdateAnimationState();
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            float wallJumpDirection = isWallSliding ? -Mathf.Sign(dirX) : 0f;
            rb.AddForce(new Vector2(wallJumpDirection * wallJumpForce, jumpForce), ForceMode2D.Impulse);

            isJumping = false;
            isWallJumping = false;
        }
    }

    public void DoWallJump()
    {
        if (isWallSliding)
        {
            isJumping = true;
            isWallSliding = false;
            isWallJumping = true;
        }
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (isJumping)
        {
            state = MovementState.jumping;
        }

        anim.SetInteger("state", (int)state);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isWallSliding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isWallSliding = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
