using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CommonBubble : MonoBehaviour
{
    [SerializeField] protected ParticleSystem movingEffect = null;
    [SerializeField] protected GameObject explodeEffect = null;

    [SerializeField] private float kockbackForce = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            Instantiate(explodeEffect).GetComponent<Transform>().position = transform.position;

            Player player_ = collision.gameObject.AddComponent<Player>();

            player_.shouldLerpMovement = true;
            player_.Jump(collision.gameObject.transform.position - transform.position, kockbackForce); 

            Destroy(gameObject);
        }
    }
}

