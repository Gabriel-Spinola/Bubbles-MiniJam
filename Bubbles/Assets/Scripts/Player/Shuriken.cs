using UnityEngine;
using Resources;
using System.Collections;

public class Shuriken : MonoBehaviour
{
    public int life = 3;

    public float speed = 10f;
    public float reflectForce = 20f;

    [Range(0.1f, 1f)]
    [SerializeField] private float colRadius = 0.32f;

    private Collider2D col;

    void Update()
    {
        Vector3 dir = StaticRes.LookDir(transform.position, true);

        transform.Translate(dir.normalized * speed * Time.deltaTime);

        Collider2D col = Physics2D.OverlapCircle(transform.position, colRadius);

        if (col /*&& col.gameObject.layer == 8*/) {
            Vector3 reflectDir = col.gameObject.transform.position - transform.position;

            Debug.Log($"Collided: { col.gameObject.name }");

            transform.Translate(reflectDir.normalized * reflectForce * Time.deltaTime);
        }
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
