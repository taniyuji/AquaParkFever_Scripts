using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using DG.Tweening;

//ゲートのアニメーション周りを制御するスクリプト
public class GateAnimation : MonoBehaviour
{
    private int passLevel;

    private IncomeUIAnimation incomeUIAnimation;

    private Sequence sequence;

    private AudioSource gateSE;

    void Awake()
    {
        incomeUIAnimation = GetComponent<IncomeUIAnimation>();
    }

    void Start()
    {
        gateSE = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(transform.localScale + Vector3.one, 0.1f))
                .Append(transform.DOScale(transform.localScale, 0.1f));
    }
    
    void OnTriggerEnter(Collider other)
    {
        //ゲートを通過したプレイヤーのマージ段階によってお金の増減やドルUIを変更する。
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

        sequence.Append(transform.DOScale(transform.localScale + Vector3.one * 0.3f, 0.1f))
                .Append(transform.DOScale(transform.localScale, 0.1f));
    }
}
