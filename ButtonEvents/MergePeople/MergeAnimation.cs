using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PathCreation.Examples;

//マージ時のキャラクターのアニメーションを管理するスクリプト
public class MergeAnimation : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private TrailRenderer trailRenderer;

    private Sequence sequence;

    private PathFollower follower;

    private CharacterBehavior characterBehavior;

    private SwimRingBehavior swimRing;

    private Vector3 defaultRotation;

    void Awake()
    {
        follower = GetComponent<PathFollower>();

        characterBehavior = GetComponent<CharacterBehavior>();

        swimRing = GetComponent<SwimRingBehavior>();

        defaultRotation = transform.localEulerAngles;
    }

    //マージされる方かする方かで処理を分ける。
    public void JudgeMerge(bool isBeMerged)
    {
        if(isBeMerged) StartCoroutine(BeMerged());
        else StartCoroutine(Merge());
    }
    
    //マージする方の処理
    //キャラをカメラ前まで動かしてキャラのレベルを上げて数秒後にスライダーに戻す。
    public IEnumerator Merge()
    {
        yield return null;

        sequence = DOTween.Sequence();

        var mergeTransform = ResourceProvider.i.mergeTransform;

        trailRenderer.gameObject.SetActive(true);

        sequence.Append(transform.DOMove(mergeTransform[0].position, moveSpeed))
                .Append(transform.DOMove(mergeTransform[2].position, moveSpeed).SetDelay(0.3f))
                .AppendCallback(() => 
                { 
                    swimRing.SetSwimRing();
                    trailRenderer.gameObject.SetActive(false);
                })
                .Append(transform.DOMove(ResourceProvider.i.addSliderBehavior.nowSliderEntrancePosition
                                         ,moveSpeed).SetDelay(0.5f))
                .AppendCallback(() =>
                {
                    characterBehavior.enabled = true;
                    transform.localEulerAngles = defaultRotation;

                    if (characterBehavior.state == CharacterBehavior.AnimationNames.OnSlider)
                    {
                        follower.enabled = true;

                        characterBehavior.PlayAnimation(CharacterBehavior.AnimationNames.OnSlider);
                    }
                    else
                    {
                        characterBehavior.SetRowWaiting();
                    }
                });
    }

    //マージされる方の処理
    //される方と衝突する瞬間にActiveをfalseにする。
    public IEnumerator BeMerged()
    {
        yield return null;

        sequence = DOTween.Sequence();

        var mergeTransform = ResourceProvider.i.mergeTransform;

        trailRenderer.gameObject.SetActive(true);

        sequence.Append(transform.DOMove(mergeTransform[1].position, moveSpeed))
                .Append(transform.DOMove(mergeTransform[2].position, moveSpeed).SetDelay(0.3f))
                .AppendCallback(() =>
                {
                    characterBehavior.enabled = true;
                    gameObject.SetActive(false);

                    trailRenderer.gameObject.SetActive(false);
                });
    }
}
