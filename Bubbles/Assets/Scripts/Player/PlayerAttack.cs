using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform attackPoint = null;
    [SerializeField] private GameObject projectile = null;

    [SerializeField] private LayerMask whatIsEnemy = 0;

    [Header("Config")]
    [Tooltip("Defines how far the melee attack point will get from player")]
    [Range(1f, 6f)]
    public float attackPointRotRadius = 1f;

    [Header("Shuriken")]
    [SerializeField] private float damage = 15f;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float cooldown = .5f;

    [SerializeField] private int maxAmountOfShurikens = 3;

    private Player player = null;
    private Shuriken shuriken = null;

    private float angle = 0f;

    private int shurikensUsed = 0;

    private bool isAShuriken = false;
    private bool canShoot = true;

    private void Awake() => player = GetComponent<Player>();

    private void Update()
    {
        angle = StaticRes.LookDir(transform.position);

        if (InputManager.I.btnThrowShuriken && !isAShuriken && shurikensUsed < maxAmountOfShurikens && canShoot) {
            Shoot();

            shurikensUsed++;

            StartCoroutine(Cooldown(duration + cooldown));
        }
        
        Aim();

        player.isAShuriken = isAShuriken;

        if (isAShuriken) {
            transform.position = shuriken.transform.position;
        }
    }

    private void Aim()
    {
        Vector3 v3Pos = Quaternion.AngleAxis(angle, Vector3.forward) * ( Vector3.right * attackPointRotRadius );

        attackPoint.position = transform.position + v3Pos;
    }

    private void Shoot()
    {
        shuriken = Instantiate(projectile, attackPoint.position, Quaternion.identity).GetComponent<Shuriken>();

        StartCoroutine(EnableIsAShuriken(duration));
        StartCoroutine(shuriken.DieOnTimer(duration));
    }

    private IEnumerator EnableIsAShuriken(float time)
    {
        isAShuriken = true;

        yield return new WaitForSeconds(time);

        isAShuriken = false;
    }
    
    private IEnumerator Cooldown(float time)
    {
        canShoot = false;

        yield return new WaitForSeconds(time);

        canShoot = true;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 v3Pos = Quaternion.AngleAxis(angle, Vector3.forward) * ( Vector3.right * attackPointRotRadius );

        Gizmos.DrawWireSphere(transform.position + v3Pos, 0.4f);
    }
}
