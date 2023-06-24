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
    private bool isJumping = false;
    private bool isDoubleJumping = false;
    private bool isWallJumping = false;
    private bool canDoubleJump = false;
    private bool left = default;

    public float wallJumpForce = 10f;
    public float wallSlideSpeed = 2f;
    public float moveSpeed = 7f;
    public float jumpForce = 10f;

    private float dirX = 0f;
    private float wallJumpDirX = 0f;
    private float originalMoveSpeed;

    [SerializeField] private bool isOnIce = false;

    private Collision2D wallCollision;

    private enum MovementState { idle, running, jumping, doubleJumping }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        originalMoveSpeed = moveSpeed;

        Application.targetFrameRate = 30;
    }

    private void Update()
    {
        if(IsGrounded()){
            isJumping = false;
            isDoubleJumping = false;
            isWallJumping = false;
            wallJumpDirX = 0f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            left = true;
            dirX = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            left = false;
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
            else if (isWallSliding)
            {
                DoWallJump();
            }
            else if (canDoubleJump)
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
        else if (isWallJumping)
        {
            // Verifica se deve sobrescrever a direção do pulo da parede caso o usuario permaneca apertando uma tecla
            // TODO: adicionar um tempo onde a direção nao conta para evitar que o usuario que ficou segurando o botao direcional nao saia da parede
            if (dirX != 0f)
            {
                rb.velocity = new Vector2(dirX * originalMoveSpeed, rb.velocity.y);
            } else {
                rb.velocity = new Vector2(wallJumpDirX * originalMoveSpeed, rb.velocity.y);
            }
        }
        else
        {
            rb.velocity = new Vector2(dirX * originalMoveSpeed, rb.velocity.y);
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
        Collider2D collider = wallCollision.collider;
        Vector2 direction = (collider.transform.position - transform.position).normalized;
        float wallJumpDirection = isWallSliding ? Mathf.Sign(direction.x) : 0f;
        
        Vector2 wallJump = new Vector2(wallJumpDirection * originalMoveSpeed, jumpForce);

        rb.velocity = wallJump;

        wallJumpDirX = wallJumpDirection;
        dirX = wallJumpDirection;

        isJumping = true;
        isWallSliding = false;
        isWallJumping = true;
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
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private IEnumerator IceDelay(float delayTime)
    {
        //Debug.Log("entrou no delay");
        yield return new WaitForSeconds(delayTime);
        //Debug.Log("saiu");
        isOnIce = false;
    }

    public void Warp(Vector2 newPos){
        this.transform.position = newPos;
    }

}
