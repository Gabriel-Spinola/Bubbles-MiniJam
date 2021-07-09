using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SufoBubble : Enemy
{
    [Header("This References")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackRate = 6f;
    [SerializeField] private float duration = 2f;
    [SerializeField] private float deatachForce = 600f;

    private Player player = null;
    private Collider2D collider = null;

    private bool shouldAttach = false;

    private float nextAttackTime = 0f;

    protected override void Awake()
    {
        base.Awake();

        player = playerTransform.gameObject.GetComponent<Player>();
        collider = GetComponent<CircleCollider2D>();
    }

    protected override void Update()
    {
        base.Update();

        if (Time.time >= nextAttackTime) {
            if (enemyState == ENEMY_STATE.ATTACKING) {
                Attack();

                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        if (shouldAttach) {
            transform.position = player.transform.position;
        }
    }

    protected override void Attack()
    {
        player.TakeDamage(damage);
    }

    private IEnumerator AttachDeatach()
    {
        shouldAttach = true;
        collider.enabled = false;

        yield return new WaitForSeconds(duration);

        transform.DOMoveY(1f, 1f);

        shouldAttach = false;
        collider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            StartCoroutine(AttachDeatach());
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
