using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();  
        ParticleSystem.MainModule main = ps.main; 
        main.stopAction = ParticleSystemStopAction.Destroy;
    }
}
