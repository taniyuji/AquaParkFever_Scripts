using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particle;

    private AudioSource mergeSE;

    void Awake()
    {
        mergeSE = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        int num;

        if (int.TryParse(other.gameObject.tag, out num))
        {
            particle.Play();

            mergeSE.Play();
        }
    }
}
