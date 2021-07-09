using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem dustEffect;

    [SerializeField] private LayerMask whatIsColEnv = 0;

    [Header("Stats")]
    [SerializeField] private float health;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float friction = 0.25f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float wallJumpForce = 5f;
    [SerializeField] private float wallJumpLerp = 10;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Collision")]
    [SerializeField] private Vector3 bottomColOffset;
    [SerializeField] private Vector3 rightColOffset;
    [SerializeField] private Vector3 leftColOffset;

    [Range(0.1f, 1f)]
    [SerializeField] private float colRadius = 0.32f;
    [Range(0.1f, 1f)]
    [SerializeField] private float bottomColRadius = 0.5f;

    public static Player I { get; private set; }

    private Rigidbody2D rb;

    private bool isGrounded = false;
    private bool isRightWall = false;
    private bool isLeftWall = false;
    private bool isOnWall = false;

    private bool canMove = true;
    private bool wallJumped = false;

    private float lookAngle;
    
    private int canJump = 0;

    private void Awake() 
    {
        I = this;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        lookAngle = StaticRes.LookDir(transform.position);

        Collision();
        FlipSprite();
        Movement();
        BetterJumping();

        canJump -= 1;

        if (isGrounded) {
            canJump = 10;

            wallJumped = false;
        }

        if (canJump > 0 && InputManager.I.keyJump)
            Jump();
        if (isOnWall && !isGrounded && InputManager.I.keyJump)
            WallJump();
    }

    private void Movement()
    {
        if (!canMove)
            return;

        if (InputManager.I.xAxis != 0) {
            if (wallJumped) {
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(InputManager.I.xAxis * moveSpeed, rb.velocity.y), wallJumpLerp * Time.deltaTime);
            }
            else {
                rb.velocity = new Vector2(InputManager.I.xAxis * moveSpeed, rb.velocity.y);
            }
        }
        else {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, friction), rb.velocity.y);
        }
    }

    private void Jump()
    {
        dustEffect.Play();

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpForce;

        canJump = 0;
    }
    
    public void Jump(float jumpForce)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpForce;

        canJump = 0;
    }

    private void WallJump()
    {
        dustEffect.Play();
        wallJumped = true;

        StopCoroutine(DisableMovement(0f));
        StartCoroutine(DisableMovement(0.1f));

        Vector2 wallDir = isLeftWall ? Vector2.right : Vector2.left;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += (Vector2.up / 1.5f + wallDir / 1.5f) * wallJumpForce;
    }

    private void Collision()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position + bottomColOffset, bottomColRadius, whatIsColEnv);
        isRightWall = Physics2D.OverlapCircle(transform.position + rightColOffset, colRadius, whatIsColEnv);
        isLeftWall = Physics2D.OverlapCircle(transform.position + leftColOffset, colRadius, whatIsColEnv);

        isOnWall = isRightWall || isLeftWall;
    }

    private void BetterJumping()
    {
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void FlipSprite() 
    { 
        transform.localScale = lookAngle < 90 && lookAngle > -90 ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
    }

    public void TakeDamage(float damage) => health -= damage;

    public IEnumerator DisableMovement(float time)
    {
        canMove = false;

        yield return new WaitForSeconds(time);

        canMove = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + rightColOffset, colRadius);
        Gizmos.DrawWireSphere(transform.position + leftColOffset, colRadius);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position + bottomColOffset, bottomColRadius);
    }
}
