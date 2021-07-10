using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryBubble : MonoBehaviour
{
    [SerializeField] private ParticleSystem movingEffect = null;
    [SerializeField] private GameObject explodeEffect = null;

    [SerializeField] private LayerMask whatIsWall = 8;

    [SerializeField] private float speed = 10f;
    [SerializeField] private float duration = 1f;
    [Range(0.1f, 1f)]
    [SerializeField] private float sphereRadius = 0.5f;

    private Player player = null;

    private int xDir = 0;
    private int yDir = 0;

    private bool xHasChoosed = false;
    private bool yHasChoosed = false;

    bool start = false;

    private void Awake() => player = GameObject.Find("[Player]").GetComponent<Player>();

    private void Update()
    {
        if (start) {
            player.transform.position = transform.position;

            if (Physics2D.OverlapCircle(transform.position, sphereRadius, whatIsWall)) {
                Explode();
            }

            if (InputManager.I.keyJump) {
                Explode();
            }

            if (InputManager.I.keyD && !xHasChoosed) {
                xDir = 1;

                xHasChoosed = true;
            }

            if (InputManager.I.keyA && !xHasChoosed) {
                xDir = -1;

                xHasChoosed = true;
            }

            if (InputManager.I.keyW && !yHasChoosed) {
                yDir = 1;

                yHasChoosed = true;
            }

            if (InputManager.I.keyS && !yHasChoosed) {
                yDir = -1;

                yHasChoosed = true;
            }
        }
    }

    private void Move()
    {
        player.canMove = false;

        if (!InputManager.I.keyJump) {
            player.GetRigidbody().gravityScale = 0;
        }

        if (xHasChoosed || yHasChoosed)
            StartCoroutine(Explode(duration));

        transform.Translate(transform.right * xDir * speed * Time.deltaTime);
        transform.Translate(transform.up * yDir * speed * Time.deltaTime);
    }

    private IEnumerator Explode(float duration)
    {
        yield return new WaitForSeconds(duration);

        player.canMove = true;
        player.shouldLerpMovement = true;
        player.GetRigidbody().gravityScale = 1;

        player.Jump(Vector2.up, 10f);

        Instantiate(explodeEffect).GetComponent<Transform>().position = transform.position;
        Destroy(gameObject);
    }
    
    private void Explode()
    {
        player.canMove = true;
        player.shouldLerpMovement = true;
        player.GetRigidbody().gravityScale = 1;

        player.Jump(Vector2.up, 10f);

        Instantiate(explodeEffect).GetComponent<Transform>().position = transform.position;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        start = true;

        if (!collision.gameObject.CompareTag("Player")) {
            Explode();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            Move();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
    }
}
