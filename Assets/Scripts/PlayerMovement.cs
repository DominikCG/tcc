using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;
    private bool isWallSliding = false;
    private bool isJumping = false;
    private bool isDoubleJumping = false;
    private bool isWallJumping = false;
    private bool canDoubleJump = false;
    private bool left = default;

    public float wallJumpForce = 10f;
    public float wallSlideSpeed = 2f;
    public float moveSpeed = 7f;
    public float jumpForce = 50f;
    private bool hasBeenGrounded = false;


    private float dirX = 0f;
    private float wallJumpDirX = 0f;
    private float originalMoveSpeed;
    private Vector2 moveTargetPointerInput;
    [SerializeField] private bool isOnIce = false;

    private Collision2D wallCollision;

    [SerializeField] private InputActionReference move;

    private enum MovementState { idle, running, jumping, doubleJumping }

    private void OnEnable()
    {
        move.action.performed += MoveInput;
    }
    private void OnDisable()
    {
        move.action.performed -= MoveInput;
    }
    private void MoveInput(InputAction.CallbackContext callbackContext)
    {
       Vector2 input = callbackContext.ReadValue<Vector2>();

        float joystickThreshold = 0.2f; // Ajuste este valor conforme necessário
        if (input.magnitude > joystickThreshold)
        {
            moveTargetPointerInput = input;
        }
        else
        {
            moveTargetPointerInput = Vector2.zero;
        }

    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        originalMoveSpeed = moveSpeed;

        Application.targetFrameRate = 30;
    }

    private void FixedUpdate()
    {
        dirX = moveTargetPointerInput.x;
        if (dirX > 0.2f)
        {
            left = false;
        }else if (dirX < -0.2f)
        {
            left = true;
        }

        if (Mathf.Abs(dirX) < 0.1f)
        {
            // Se o valor absoluto de dirX for menor que 0.1, considere que o joystick está solto
            dirX = 0f; // Pare o movimento do personagem
        }
        else
        {
            rb.velocity = new Vector2(dirX * originalMoveSpeed, rb.velocity.y);
        }
        if (IsGrounded()){
            isJumping = false;
            isDoubleJumping = false;
            isWallJumping = false;
            canDoubleJump = false;
            wallJumpDirX = 0f;
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        if(moveTargetPointerInput.y > 0.7f)
        {
            if (IsGrounded())
            {
                Jump();
                StartCoroutine(TimerDoubleJump(0.3f));
            }
            else if (isWallSliding)
            {
                DoWallJump();
            }
            if (canDoubleJump)
            {
                Jump();
                canDoubleJump = false;
            }
            
        }

        if (isOnIce)
        {
            if(left){
                rb.velocity = new Vector2(-moveSpeed * 0.6f, rb.velocity.y);
            }else{
                rb.velocity = new Vector2(moveSpeed * 0.6f, rb.velocity.y);
            }
        }
        

        UpdateAnimationState();
    }

    private void Jump()
    {
        if (isJumping)
        {
            isDoubleJumping = true;

        }
        
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
    }

    public void DoWallJump()
    {
        if (hasBeenGrounded) // Verifica se o personagem já esteve no chão
        {
            Collider2D collider = wallCollision.collider;
            Vector2 direction = (collider.transform.position - transform.position).normalized;
            float wallJumpDirection = isWallSliding ? Mathf.Sign(direction.x) : 0f;

            Vector2 wallJump = new Vector2(wallJumpDirection * originalMoveSpeed, jumpForce);

            //rb.velocity = wallJump;

            // Aplica a força ao Rigidbody2D
            rb.AddForce(wallJump, ForceMode2D.Impulse);

            wallJumpDirX = wallJumpDirection;
            dirX = wallJumpDirection;
            hasBeenGrounded = false;
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

        if (isDoubleJumping)
        {
            state = MovementState.doubleJumping;
        }
        else if (isJumping)
        {
            state = MovementState.jumping;
        }

        anim.SetInteger("state", (int)state);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {   
            isOnIce = false;
            isWallSliding = true;
            wallCollision = collision;
        }
        if (collision.gameObject.CompareTag("Ice"))
        {
            //Debug.Log("entrou");
            isOnIce = true;
            moveSpeed = originalMoveSpeed;
        }
        if (collision.gameObject.tag != "Ice")
        {
            isOnIce = false;
            moveSpeed = originalMoveSpeed;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ice"))
        {
            isOnIce = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isWallSliding = false;
        }

        if (collision.gameObject.CompareTag("Ice"))
        {   
            //Debug.Log("saiu do gelo");
            StartCoroutine(IceDelay(1f));
        }

    }

    private bool IsGrounded()
    {
        bool grounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);

        if (grounded)
        {
            hasBeenGrounded = true;
        }

        return grounded;
    }

    private IEnumerator IceDelay(float delayTime)
    {
        //Debug.Log("entrou no delay");
        yield return new WaitForSeconds(delayTime);
        //Debug.Log("saiu");
        isOnIce = false;
    }

    private IEnumerator TimerDoubleJump(float delayTime)
    {
        //Debug.Log("entrou no delay");
        yield return new WaitForSeconds(delayTime);
        //Debug.Log("saiu");
        canDoubleJump = true;
    }

    public void Warp(Vector2 newPos){
        this.transform.position = newPos;
    }

}
