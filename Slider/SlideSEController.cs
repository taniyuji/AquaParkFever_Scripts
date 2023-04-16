using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideSEController : MonoBehaviour
{
    [SerializeField]
    private AudioSource slideSE;

    private int judgeNumber;

    void OnTriggerStay(Collider other)
    {
        if (int.TryParse(other.gameObject.tag, out judgeNumber))
        {
            if (slideSE.isPlaying) return;

            slideSE.Play();
        }
        else
        {
            slideSE.Pause();
        }
    }


}
