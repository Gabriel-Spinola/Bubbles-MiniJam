using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Resources;

public class Shuriken : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    // Update is called once per frame
    void Update()
    {
        float lookAngle = StaticRes.LookDir(transform.position);
        Vector3 dir = StaticRes.LookDir(transform.position, true);

        transform.Translate((dir).normalized * speed * Time.deltaTime);

        //transform.localScale = lookAngle < 90 && lookAngle > -90 ? new Vector3(1f, 1f, 1f) : new Vector3(-1f, 1f, 1f);
    }
}
