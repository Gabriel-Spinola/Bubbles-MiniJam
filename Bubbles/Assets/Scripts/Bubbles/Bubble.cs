using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bubble : MonoBehaviour
{
    [SerializeField] protected ParticleSystem movingEffect;
    [SerializeField] protected GameObject explodeEffect;

    protected abstract void Behaviour();
}
