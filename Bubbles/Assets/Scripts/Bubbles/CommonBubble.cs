using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CommonBubble : Bubble
{
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        Behaviour();
    }

    protected override void Behaviour()
    {
        //throw new System.NotImplementedException();
        //transform.DOMoveY(0.5f, 1f);
       // transform.DORestart();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            Instantiate(explodeEffect).GetComponent<Transform>().position = transform.position;

            Destroy(gameObject);
        }
    }
}

