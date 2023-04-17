using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//キャラがプールに飛び込んだ際の水飛沫エフェクトを管理するスクリプト
public class PoolSplashController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem splash;

    private List<ParticleSystem> splashes = new List<ParticleSystem>();

    private bool checkInterval;

    private AudioSource splashSE;

    private List<ParticleSystem> splashList = new List<ParticleSystem>();

    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        splashSE = GetComponent<AudioSource>();

        splashList.Add(splash);

        splash.gameObject.SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            var instantiatedSplash = Instantiate(splash, transform);

            instantiatedSplash.gameObject.SetActive(false);

            splashList.Add(instantiatedSplash);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (checkInterval) return;

        int target;

        if (int.TryParse(other.gameObject.tag, out target))
        {
            if (splashList[index].isPlaying)
            {
                index = index >= splashList.Count - 1 ? 0 : index += 1;
            }

            //Debug.Log("index = " + index);

            splashList[index].gameObject.SetActive(true);

            splashSE.PlayOneShot(splashSE.clip);

            StartCoroutine(CheckInterval());
        }
    }

    private IEnumerator CheckInterval()
    {
        checkInterval = true;

        yield return null;

        checkInterval = false;
    }
}
