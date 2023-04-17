using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//マージ時の浮き輪アニメーションを制御するスクリプト
public class SwimRingAnimation : MonoBehaviour
{
    [SerializeField]
    private float expandSpeed;

    private Vector3 defaultScale;

    private Sequence sequence;

    void Awake()
    {
        defaultScale = transform.localScale;
    }

    void Start()
    {
        sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(new Vector3(defaultScale.x + 1f, defaultScale.y + 1f, defaultScale.z + 1f), expandSpeed))
                .Append(transform.DOScale(new Vector3(defaultScale.x, defaultScale.y, defaultScale.z), expandSpeed));
    }
}
