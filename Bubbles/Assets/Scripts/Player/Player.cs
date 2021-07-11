using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem dustEffect = null;
    [SerializeField] private AudioManager audioManager = null;

    [SerializeField] private LayerMask whatIsColEnv = 0;

    [Header("Stats")]
    [SerializeField] private float health = 10f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float friction = 0.25f;
    [SerializeField] private float lerpMovement = 10;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Walljump")]
    [SerializeField] private float wallJumpForce = 5f;
    [SerializeField] private int amountOfWallJumpsPossible = 3;

    [Header("Collision")]
    [SerializeField] private Vector3 bottomColOffset = Vector2.zero;
    [SerializeField] private Vector3 rightColOffset = Vector2.zero;
    [SerializeField] private Vector3 leftColOffset = Vector2.zero;

    [Range(0.1f, 1f)]
    [SerializeField] private float colRadius = 0.32f;
    [Range(0.1f, 1f)]
    [SerializeField] private float bottomColRadius = 0.5f;

    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public bool isRightWall = false;
    [HideInInspector] public bool isLeftWall = false;
    [HideInInspector] public bool isOnWall = false;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool shouldLerpMovement = false;
    [HideInInspector] public bool isAShuriken = false;

    public static Player I { get; private set; }

    private Rigidbody2D rb = null;

    private Vector2 wallDir = Vector2.zero;

    private bool wallJumped = false;
    private bool useBetterJump = true;

    private int wallJumpCount = 0;

    private float lookAngle = 0f;
    
    private int canJump = 0;

    private void Awake() 
    {
        I = this;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isAShuriken) {
            transform.GetChild(0).gameObject.SetActive(true);

            Collision();
            FlipSprite();
            Movement();
            BetterJumping();

            canJump -= 1;

            if (isGrounded) {
                canJump = 10;

                wallJumped = false;
                wallJumpCount = 0;
                shouldLerpMovement = false;
                useBetterJump = true;
            }

            if (canJump > 0 && InputManager.I.keyJump)
                Jump();
            if (isOnWall && !isGrounded && InputManager.I.keyJump && wallJumpCount < amountOfWallJumpsPossible)
                WallJump();
        }
        else {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void Movement()
    {
        if (!canMove)
            return;

        if (wallJumped || shouldLerpMovement) {
            rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(InputManager.I.xAxis * moveSpeed, rb.velocity.y), lerpMovement * Time.deltaTime);
        }

        if (InputManager.I.xAxis != 0) {
            rb.velocity = new Vector2(InputManager.I.xAxis * moveSpeed, rb.velocity.y);
        }
        else {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, friction), rb.velocity.y);
        }
    }

    private void Jump()
    {
        audioManager.Play("PlayerJump");
        dustEffect.Play();

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpForce;

        canJump = 0;
    }
    
    public void Jump(Vector2 dir, float jumpForce)
    {
        audioManager.Play("PlayerJump");

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;

        useBetterJump = false;

        canJump = 0;
    }

    private void WallJump()
    {
        dustEffect.Play();
        wallJumped = true;

        StopCoroutine(DisableMovement(0f));
        StartCoroutine(DisableMovement(0.1f));

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += (Vector2.up / 1.5f + wallDir / 1.5f) * wallJumpForce;

        wallJumpCount += 1;
    }

    private void Collision()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position + bottomColOffset, bottomColRadius, whatIsColEnv);
        isRightWall = Physics2D.OverlapCircle(transform.position + rightColOffset, colRadius, whatIsColEnv);
        isLeftWall = Physics2D.OverlapCircle(transform.position + leftColOffset, colRadius, whatIsColEnv);

        isOnWall = isRightWall || isLeftWall;

        wallDir = isLeftWall ? Vector2.right : Vector2.left;
    }

    private void BetterJumping()
    {
        if (!useBetterJump)
            return;

        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void FlipSprite() => transform.localScale = InputManager.I.xAxis < 0 ? new Vector2(-1f, 1f) : (
        InputManager.I.xAxis > 0 ? new Vector2(1f, 1f) : new Vector2(transform.localScale.x, 1f)
    );

    public void TakeDamage(float damage) => health -= damage;

    public IEnumerator DisableMovement(float time)
    {
        canMove = false;

        yield return new WaitForSeconds(time);

        canMove = true;
    }

    public Rigidbody2D GetRigidbody() => rb;
    public void SetBetterJump(bool val) => useBetterJump = val;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 31) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spikes")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (collision.CompareTag("Player Spikes")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + rightColOffset, colRadius);
        Gizmos.DrawWireSphere(transform.position + leftColOffset, colRadius);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position + bottomColOffset, bottomColRadius);
    }
}
