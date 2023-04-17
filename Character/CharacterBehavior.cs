using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using PathCreation.Examples;
using DG.Tweening;

//キャラクターの動作を制御するスクリプト
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
    }

    void OnEnable()
    {
        isMerged = false;
        follower.enabled = false;

        //列に待機している他のキャラがいるかどうか
        if (!ResourceProvider.i.waitRow.isWaiting)
        {
            //いない場合はスライダーを滑り始める
            GoSlideBehavior();

            //Debug.Log("enabledAgain&GoSlider" + gameObject.name);
        }
        else
        {
            //いた場合は待機列に追加する。
            SetRowWaiting();

            //Debug.Log("enabledAgain&waitRow" + gameObject.name);
        }
    }

    void Start()
    {
        //全てのパスを滑り終わった場合、プール落下処理を開始する。
        follower.getEndOfPath.Subscribe(i =>
        {
            if (sequence == null || !sequence.IsPlaying())
            {
                FromFallPoolBehavior();
                //Debug.Log("Play Sequence");
            }
        });

        //スライダーのレベルが上がった場合待機列の位置を移動する。
        ResourceProvider.i.addSliderEventController.SliderLevelChanged.Subscribe((Action<int>)(i =>
        {
            if (sequence != null && sequence.IsPlaying())
            {
                this.SetRowWaiting();
            }
        }));

        //待機列から滑り始めるキャラをそのFollowerとともに通知する
        ResourceProvider.i.waitRow.goSlide.Subscribe(i =>
        {
            if (!isWaiting) return;

            if (isMerged) return;

            //通知されたFollowerが自分自身のものでなかった場合は列を進む
            if (i != follower)
            {
                PlayAnimation(AnimationNames.Run);

                sequence = DOTween.Sequence();

                var sliderBack = ResourceProvider.i.addSliderBehavior.nowBackSide;

                var defaultRowInterval = ResourceProvider.i.waitRow.getDefaultRowInterval;

                var rowInterval = sliderBack * defaultRowInterval;

                sequence.Append(transform.DOMove(transform.position + rowInterval, 0.4f).SetDelay(0.3f));
            }
            else//Followerが自分自身だった場合滑り始める。
            {
                GoSlideBehavior();
            }
        });
    }

    //待機列追加処理
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

    //プール落下から梯子を登り切るまでの処理
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

                //プールに潜ってから上がるまでの処理
        sequence.Append(transform.DOMove(new Vector3(defaultX + 1f, defaultY - 3, defaultZ), moveSpeed))
                .Append(transform.DOMove(new Vector3(defaultX + 1f, poolLadderTransform.position.y, defaultZ), moveSpeed))
                .AppendCallback(() =>
                {
                    PlayAnimation(AnimationNames.Swim);
                    transform.LookAt(poolLadderTransform);
                })
                //プールの梯子まで泳ぐ処理
                .Append(transform.DOMove(poolLadderTransform.position, moveSpeed * 4).SetEase(Ease.InSine))
                .AppendCallback(() =>
                 {
                     PlayAnimation(AnimationNames.Climb);
                     transform.position = climbLadderPosition;
                     var slider = ResourceProvider.i.addSliderBehavior.nowSliderEntrancePosition;
                     transform.LookAt(new Vector3(slider.x, climbLadderPosition.y, slider.z));

                 })
                 //スライダーの足場まで登る処理
                 .Append(transform.DOMove(basePosition, 2f).SetEase(Ease.InSine))
                 .AppendCallback(() =>
                  {
                      //待機している他のキャラがいた場合飛び込み口までは進まず待機列に並ぶ。
                      if (ResourceProvider.i.waitRow.isWaiting)
                      {
                          SetRowWaiting();
                    
                      }

                      PlayAnimation(AnimationNames.Run);
                      transform.position = new Vector3(transform.position.x, sliderPosition.y, transform.position.z);
                  })
                  //スライダーの飛び込む口まで進む処理
                  .Append(transform.DOMove(sliderPosition, 1f).SetEase(Ease.InSine))
                  .AppendCallback(() =>
                  {
                      //他に待機しているキャラがいた場合は待機列へ。それ以外は飛び込む。
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

        //滑るアニメーションじゃない場合は他に滑っている人がいない確認する処理を実行する。
        if (name != AnimationNames.OnSlider)
        {
            ResourceProvider.i.peoplePool.CheckNoOneOnSlider();
        }
        else
        {
            //滑るアニメーションの場合は滑っている効果音をプレイする。
            ResourceProvider.i.peoplePool.playSE();
        }
    }

    //マージ対象に選ばれた時の処理。各パラメータをリセットしてマージアニメーションを呼び出す。
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
