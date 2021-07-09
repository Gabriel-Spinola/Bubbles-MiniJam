using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    void Start()
    {
        var s = GetComponent<ParticleSystem>();  var a = s.main; a.stopAction = ParticleSystemStopAction.Destroy;
    }
}
