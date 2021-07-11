using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 500f;

    void Update()
    {
        transform.Rotate(transform.forward * rotationSpeed * Time.deltaTime);
    }
}
