using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask whatIsColEnv = 0;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float friction = 0f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 0f;
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

        if (isGrounded) canJump = 10;
        if (canJump > 0 && InputManager.I.keyJump)
            Jump();
    }

    private void Movement()
    {
        if (InputManager.I.xAxis != 0) {
            rb.velocity = new Vector2(InputManager.I.xAxis * moveSpeed, rb.velocity.y);
        }
        else {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, friction), rb.velocity.y);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpForce;

        canJump = 0;
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

    private void FlipSprite() => transform.localScale = lookAngle < 90 && lookAngle > -90 ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + rightColOffset, colRadius);
        Gizmos.DrawWireSphere(transform.position + leftColOffset, colRadius);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position + bottomColOffset, bottomColRadius);
    }
}
