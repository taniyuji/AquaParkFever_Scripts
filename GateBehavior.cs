using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using DG.Tweening;

public class GateBehavior : MonoBehaviour
{
    private int passLevel;

    private IncomeUIAnimation incomeUIAnimation;

    private Sequence sequence;

    private Vector3 defaultScale = Vector3.zero;

    private AudioSource gateSE;

    void Awake()
    {
        incomeUIAnimation = GetComponent<IncomeUIAnimation>();
    }

    void Start()
    {
        defaultScale = transform.localScale;

        gateSE = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(new Vector3(defaultScale.x + 1f, defaultScale.y + 1, defaultScale.z + 1), 0.1f))
                .Append(transform.DOScale(defaultScale, 0.1f));
    }

    void OnTriggerEnter(Collider other)
    {
        if (int.TryParse(other.gameObject.tag, out passLevel))
        {
            // Debug.Log("parsedLevel = " + passLevel);

            ResourceProvider.i.informationManager.AddDollar(passLevel);

            incomeUIAnimation.MoveIncomeUI(passLevel);

            PlaySequence();

            gateSE.pitch = 3 - passLevel;

            gateSE.PlayOneShot(gateSE.clip);

        }
        else
        {
            //Debug.Log("notInt");
        }
    }

    private void PlaySequence()
    {
        sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(new Vector3(defaultScale.x + 0.3f, defaultScale.y + 0.3f, defaultScale.z + 0.3f), 0.1f))
                .Append(transform.DOScale(defaultScale, 0.1f));
    }
}
