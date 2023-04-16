using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using PathCreation.Examples;
using DG.Tweening;

public class CharacterBehavior : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private Transform swimRing;

    [SerializeField]
    private ParticleSystem OnSliderEffect;

    private Vector3 defaultSwimRingPosition;

    public enum AnimationNames
    {
        Wait,
        OnSlider,
        FallPool,
        Swim,
        Climb,
        Run,
        JumpOnSlider,
    }

    public AnimationNames state { get; private set; }

    private Transform poolLadderTransform;

    private Vector3 climbLadderPosition;

    private Vector3 basePosition;

    private Vector3 sliderPosition;

    private Vector3 sliderJumpPosition;

    private PathFollower follower;

    private SwimRingBehavior swimRingBehavior;

    private Animator animator;

    private MergeAnimation mergeAnimation;

    private bool isWaiting = false;

    private bool isMerged = false;

    Sequence sequence;

    void Awake()
    {
        follower = GetComponent<PathFollower>();
        swimRingBehavior = GetComponent<SwimRingBehavior>();
        animator = GetComponent<Animator>();
        mergeAnimation = GetComponent<MergeAnimation>();
        poolLadderTransform = ResourceProvider.i.poolLadderTransform;
        climbLadderPosition = ResourceProvider.i.addSliderBehavior.nowClimbLadderPosition;
        basePosition = ResourceProvider.i.addSliderBehavior.nowBaseBackPosition;
        sliderPosition = ResourceProvider.i.addSliderBehavior.nowSliderEntrancePosition;
        sliderJumpPosition = ResourceProvider.i.addSliderBehavior.nowSliderJumpPosition;
    }

    void OnEnable()
    {
        isMerged = false;
        follower.enabled = false;

        if (!ResourceProvider.i.waitRow.isWaiting)
        {
            GoSlideBehavior();

            //Debug.Log("enabledAgain&GoSlider" + gameObject.name);
        }
        else
        {
            SetRowWaiting();

            //Debug.Log("enabledAgain&waitRow" + gameObject.name);
        }
    }

    void Start()
    {
        follower.getEndOfPath.Subscribe(i =>
        {
            if (sequence == null || !sequence.IsPlaying())
            {
                FromFallPoolBehavior();
                //Debug.Log("Play Sequence");
            }
        });

        ResourceProvider.i.addSliderEventController.SliderLevelChanged.Subscribe((Action<int>)(i =>
        {
            if (sequence != null && sequence.IsPlaying())
            {
                this.SetRowWaiting();
            }
        }));

        ResourceProvider.i.waitRow.goSlide.Subscribe(i =>
        {
            if (!isWaiting) return;

            if (isMerged) return;

            if (i != follower)
            {
                PlayAnimation(AnimationNames.Run);

                sequence = DOTween.Sequence();

                var sliderBack = ResourceProvider.i.addSliderBehavior.nowBackSide;

                var defaultRowInterval = ResourceProvider.i.waitRow.getDefaultRowInterval;

                var rowInterval = sliderBack * defaultRowInterval;

                sequence.Append(transform.DOMove(transform.position + rowInterval, 0.4f).SetDelay(0.3f));
            }
            else
            {
                GoSlideBehavior();
            }
        });
    }

    public void SetRowWaiting()
    {
        isWaiting = true;
        sequence.Kill();
        PlayAnimation(AnimationNames.Wait);
        ResourceProvider.i.waitRow.AddList(follower);

        //Debug.Log("wait" + gameObject.name);
    }

    private void GoSlideBehavior()
    {
        sequence = DOTween.Sequence();

        isWaiting = false;

        PlayAnimation(AnimationNames.JumpOnSlider);

        sliderJumpPosition = ResourceProvider.i.addSliderBehavior.nowSliderJumpPosition;

        // Debug.Log("First" + transform.position.y);

        transform.LookAt(new Vector3(sliderJumpPosition.x, transform.position.y, sliderJumpPosition.z));

        sequence.Append(transform.DOMove(new Vector3(sliderJumpPosition.x, transform.position.y + 1, sliderJumpPosition.z), 0.25f))
                .SetDelay(0.5f)
                .Append(transform.DOMove(new Vector3(sliderJumpPosition.x - 0.1f, sliderJumpPosition.y - 0.5f, sliderJumpPosition.z - 0.1f), 0.25f))
                .AppendCallback(() =>
                 {
                     //Debug.Log("Last" + transform.position.y);

                     PlayAnimation(AnimationNames.OnSlider);

                     follower.enabled = true;

                     follower.ResetPosition();
                 });

    }

    void FromFallPoolBehavior()
    {
        basePosition = ResourceProvider.i.addSliderBehavior.nowBaseBackPosition;

        sliderPosition = ResourceProvider.i.addSliderBehavior.nowSliderEntrancePosition;

        climbLadderPosition = ResourceProvider.i.addSliderBehavior.nowClimbLadderPosition;

        sequence = DOTween.Sequence();

        var defaultX = transform.position.x;

        var defaultY = transform.position.y;

        var defaultZ = transform.position.z;

        transform.eulerAngles = new Vector3(0, 0, 0);

        follower.enabled = false;

        PlayAnimation(AnimationNames.FallPool);

        sequence.Append(transform.DOMove(new Vector3(defaultX + 1f, defaultY - 3, defaultZ), moveSpeed))
                .Append(transform.DOMove(new Vector3(defaultX + 1f, poolLadderTransform.position.y, defaultZ), moveSpeed))
                .AppendCallback(() =>
                {
                    PlayAnimation(AnimationNames.Swim);
                    transform.LookAt(poolLadderTransform);
                })
                .Append(transform.DOMove(poolLadderTransform.position, moveSpeed * 4).SetEase(Ease.InSine))
                .AppendCallback(() =>
                 {
                     PlayAnimation(AnimationNames.Climb);
                     transform.position = climbLadderPosition;
                     var slider = ResourceProvider.i.addSliderBehavior.nowSliderEntrancePosition;
                     transform.LookAt(new Vector3(slider.x, climbLadderPosition.y, slider.z));

                 })
                 .Append(transform.DOMove(basePosition, 2f).SetEase(Ease.InSine))
                 .AppendCallback(() =>
                  {
                      if (ResourceProvider.i.waitRow.isWaiting)
                      {
                          SetRowWaiting();
                      }

                      PlayAnimation(AnimationNames.Run);
                      transform.position = new Vector3(transform.position.x, sliderPosition.y, transform.position.z);
                  })
                  .Append(transform.DOMove(sliderPosition, 1f).SetEase(Ease.InSine))
                  .AppendCallback(() =>
                  {
                      if (ResourceProvider.i.waitRow.isWaiting)
                      {
                          SetRowWaiting();
                      }
                      else
                      {
                          GoSlideBehavior();
                      }
                  });
    }

    public void PlayAnimation(AnimationNames name)
    {
        state = name;

        animator.Play(name.ToString());

        swimRingBehavior.SetSwimRingTransform(name);

        OnSliderEffect.gameObject.SetActive(name == AnimationNames.OnSlider);

        if (name != AnimationNames.OnSlider)
        {
            ResourceProvider.i.peoplePool.CheckNoOneOnSlider();
        }
        else
        {
            ResourceProvider.i.peoplePool.playSE();
        }
    }

    public void SetMerge(bool isBeMerged)
    {
        sequence.Kill();

        isMerged = true;

        animator.Play(AnimationNames.OnSlider.ToString());

        OnSliderEffect.gameObject.SetActive(false);

        transform.localEulerAngles = new Vector3(60, -40, -20);

        this.enabled = false;

        follower.enabled = false;

        mergeAnimation.JudgeMerge(isBeMerged);
    }
}
