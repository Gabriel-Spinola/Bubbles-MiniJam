using UnityEngine;
using Resources;
using System.Collections;

public class Shuriken : MonoBehaviour
{
    public LayerMask whatIsEnv;

    public int life = 3;

    public float speed = 10f;
    public float reflectForce = 20f;

    [Range(0.1f, 1f)]
    [SerializeField] private float colRadius = 0.32f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        Vector2 dir = StaticRes.LookDir(transform.position, true);
        dir.Normalize();

        rb.MovePosition((Vector2) transform.position + (dir * speed * Time.fixedDeltaTime));
    }

    public IEnumerator DieOnTimer(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, colRadius);
    }
}
