using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectEnabler : MonoBehaviour
{
    private ParticleSystem particle;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if(particle.isStopped) gameObject.SetActive(false);
    }
}
