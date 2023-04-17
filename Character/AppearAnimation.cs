using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PathCreation.Examples;
using UnityEngine.Animations;

//キャラの出現時のアニメーションを制御するスクリプト
public class AppearAnimation : MonoBehaviour
{
    [SerializeField]
    private float animationSpeed;

    [SerializeField]
    private float addScaleAmount;

    private CharacterBehavior behavior;

    private PathFollower follower;

    private Sequence sequence;

    private Vector3 defaultScale;

    private Vector3 defaultRotation;

    void Awake()
    {
        behavior = GetComponent<CharacterBehavior>();

        follower = GetComponent<PathFollower>();

        defaultScale = transform.localScale;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        sequence = DOTween.Sequence();

        //Debug.Log(rowInterval);

        sequence.Append(transform.DOScale(defaultScale + Vector3.one * addScaleAmount, animationSpeed / 2))
                .Append(transform.DOScale(defaultScale, animationSpeed / 2))
                .AppendCallback(() =>
                {
                    //Debug.Log(defaultScale);

                    behavior.enabled = true;
                });
    }
}
