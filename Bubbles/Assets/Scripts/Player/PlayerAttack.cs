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

    private Player player = null;
    private Shuriken shuriken = null;

    private float angle = 0f;

    bool shotted = false;

    private void Awake() => player = GetComponent<Player>();

    private void Update()
    {
        angle = StaticRes.LookDir(transform.position);

        if (InputManager.I.btnThrowShuriken && !shotted) {
            Shoot();
        }
        
        Aim();

        if (shotted) {
            player.isAShuriken = true;
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
        shotted = true;   
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 v3Pos = Quaternion.AngleAxis(angle, Vector3.forward) * ( Vector3.right * attackPointRotRadius );

        Gizmos.DrawWireSphere(transform.position + v3Pos, 0.4f);
    }
}
