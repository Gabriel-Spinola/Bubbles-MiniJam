using System.Collections;
using UnityEngine;
using Pathfinding;
using System;
using Resources;

public abstract class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Transform attackPoint = null;

    [SerializeField] private LayerMask whatIsWall = 8;
    [SerializeField] protected LayerMask whatIsPlayer = 0;

    [Header("Config")]
    [SerializeField] protected bool drawGizmos = true;

    [Tooltip("Defines how far the melee attack point will get from the enemy")]
    [Range(0.5f, 6f)]
    public float attackPointRotRadius = 0.5f;

    [Header("Stats")]
    [SerializeField] protected float maxHealth = 100;
    [SerializeField] protected float speed = 500f;

    [Header("State Triggers")]
    [Range(0f, 500f)]
    [SerializeField] protected float chaseRadius = 10f;
    [Range(0f, 10f)]
    [SerializeField] protected float attackRadius = 5f;
    [SerializeField] protected float nextWaypointDistance = 3f;

    [Space]

    protected Rigidbody2D rb = null;
    protected Path path = null;
    protected Seeker seeker = null;
    protected Transform playerTransform = null;

    protected enum ENEMY_STATE {
        NEUTRAL,
        CHASING,
        ATTACKING
    }

    protected ENEMY_STATE enemyState = ENEMY_STATE.NEUTRAL;

    protected Vector2 dir = Vector2.zero;

    protected float lookAngle = 0f;

    protected bool canMove = true;

    private int randomSpot = 0;
    private int currentWaypoint = 0;

    private float waitTime = 0f;
    private float currentHealth = 0f;

    protected virtual void Awake()
    {
        playerTransform = GameObject.Find("[Player]").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;

        InvokeRepeating(nameof(UpdatePath), 0f, .5f);
    }

    protected virtual void Update()
    {
        if (currentHealth <= 0)
            Die();

        lookAngle = StaticRes.LookDir(transform.position, playerTransform.position);

        CheckState();   
    }

    protected virtual void FixedUpdate()
    {
        if (canMove) {
            switch (enemyState) {
                case ENEMY_STATE.CHASING:
                    Chase();
                break;
            }
        }
    }

    protected void CheckState()
    {
        if (Physics2D.OverlapCircle(transform.position, chaseRadius, whatIsPlayer) && enemyState == ENEMY_STATE.NEUTRAL) {
            enemyState = ENEMY_STATE.CHASING; 
        }

        if (
            Physics2D.OverlapCircle(transform.position, attackRadius, whatIsPlayer) &&
            !Physics2D.Linecast(transform.position, playerTransform.position, whatIsWall) &&
            enemyState == ENEMY_STATE.CHASING
        ) {
            enemyState = ENEMY_STATE.ATTACKING;
        }

        if (enemyState == ENEMY_STATE.ATTACKING) {
            if (!Physics2D.OverlapCircle(transform.position, attackRadius, whatIsPlayer)) {
                enemyState = ENEMY_STATE.CHASING;
            }
        }

        if (enemyState == ENEMY_STATE.CHASING) {
            if (!Physics2D.OverlapCircle(transform.position, chaseRadius, whatIsPlayer)) {
                enemyState = ENEMY_STATE.NEUTRAL;
            }
        }
    }

    protected virtual void Chase()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count) {
            Debug.Log("End Of Path Reached");

            return;
        }

        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

        rb.AddForce(dir * speed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }
    }

    protected void UpdatePath()
    {
        if (enemyState == ENEMY_STATE.CHASING) {
            if (seeker.IsDone()) {
                seeker.StartPath(transform.position, playerTransform.position, (Path p) => {
                    if (!p.error) {
                        path = p;
                        currentWaypoint = 0;
                    }
                    else {
                        throw new Exception("Failure at loading path");
                    }
                });
            }
        }
    }

    protected virtual void Attack() {
        if (canMove is false)
            return;

        Vector3 v3Pos = Quaternion.AngleAxis(lookAngle, Vector3.forward) * (Vector3.right * attackPointRotRadius);

        attackPoint.position = transform.position + v3Pos;
    }

    public void TakeDamage(float damage) => currentHealth -= damage;

    public virtual void Die() => Destroy(gameObject);

    public IEnumerator DisabeMovement(float time)
    {
        canMove = false;

        yield return new WaitForSeconds(time);

        canMove = true;
    }
}
