using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CommonBubble : Bubble
{
    [SerializeField] private float kockbackForce = 5f;

    protected override void Behaviour() {}

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

