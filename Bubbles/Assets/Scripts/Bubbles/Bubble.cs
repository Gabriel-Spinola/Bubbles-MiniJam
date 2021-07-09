using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bubble : MonoBehaviour
{
    [SerializeField] protected ParticleSystem movingEffect = null;
    [SerializeField] protected GameObject explodeEffect = null;

    protected abstract void Behaviour();
}
