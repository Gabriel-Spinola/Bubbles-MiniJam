using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Shuriken : MonoBehaviour
{
    public LayerMask whatIsEnv;

    public int life = 3;

    public float speed = 10f;
    public float reflectForce = 20f;
    public bool isBreaked = false;

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

    private void Update()
    {
        if (isBreaked) {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Vector2 dir = StaticRes.LookDir(transform.position, true);
        dir.Normalize();

        //rb.MovePosition((Vector2) transform.position + (dir * speed * Time.fixedDeltaTime));

        //rb.MovePosition((Vector2) transform.position + (Vector2.right * speed * Time.fixedDeltaTime));

        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }

    public IEnumerator DieOnTimer(float time)
    {
        yield return new WaitForSeconds(time);

        try {
            Destroy(gameObject);
        } catch { }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8) {
            GameObject.Find("[Player]").GetComponent<PlayerAttack>().KillShuriken();
            GameObject.Find("ShurikenCanUse").SetActive(true);
        }

        if (collision.gameObject.CompareTag("Destructable")) {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.layer == 11) {
            GameObject.Find("[Player]").GetComponent<PlayerAttack>().KillShuriken();
        }

        if (collision.CompareTag("Spikes")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (collision.CompareTag("SSPikes")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, colRadius);
    }
}
