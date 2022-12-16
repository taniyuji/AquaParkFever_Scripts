using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PathCreation.Examples;

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

    void Awake()
    {
        follower = GetComponent<PathFollower>();

        characterBehavior = GetComponent<CharacterBehavior>();

        swimRing = GetComponent<SwimRingBehavior>();
    }

    public void JudgeMerge(bool isBeMerged)
    {
        if(isBeMerged) StartCoroutine(BeMerged());
        else StartCoroutine(Merge());
    }
    
    public IEnumerator Merge()
    {
        yield return null;

        sequence = DOTween.Sequence();

        var mergeTransform = ResourceProvider.i.mergeTransform;

        var returnPosition = characterBehavior.state == CharacterBehavior.AnimationNames.OnSlider
                     ? transform.position : ResourceProvider.i.addSliderBehavior.nowSliderEntrancePosition;

        trailRenderer.gameObject.SetActive(true);

        sequence.Append(transform.DOMove(mergeTransform[0].position, moveSpeed))
                .Append(transform.DOMove(mergeTransform[2].position, moveSpeed).SetDelay(0.3f))
                .AppendCallback(() => 
                { 
                    swimRing.SetSwimRing(characterBehavior.state);
                    trailRenderer.gameObject.SetActive(false);
                })
                .Append(transform.DOMove(returnPosition, moveSpeed).SetDelay(0.5f))
                .AppendCallback(() =>
                {
                    characterBehavior.enabled = true;
                    
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