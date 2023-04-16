using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PathCreation.Examples;
using UnityEngine.Animations;

public class AppearAnimation : MonoBehaviour
{
    [SerializeField]
    private float animationSpeed;

    [SerializeField]
    private float addScaleAmount;

    [SerializeField]
    private List<ParentConstraint> constraintsList;

    private CharacterBehavior behavior;

    private PathFollower follower;

    private Sequence sequence;

    private Vector3 defaultScale;

    void Awake()
    {
        behavior = GetComponent<CharacterBehavior>();

        follower = GetComponent<PathFollower>();

        defaultScale = transform.localScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        sequence = DOTween.Sequence();

        var entrancePosition = ResourceProvider.i.addSliderBehavior.nowSliderEntrancePosition;

        var rowInterval = ResourceProvider.i.waitRow.rowInterval;

        //Debug.Log(rowInterval);

        if (constraintsList.Count != 0)
        {
            for (int i = 0; i < constraintsList.Count; i++)
                constraintsList[i].weight = 0;
        }

        sequence.Append(transform.DOScale(new Vector3(defaultScale.x + addScaleAmount,
                                                        defaultScale.y + addScaleAmount,
                                                        defaultScale.z + addScaleAmount)
                                                        , animationSpeed / 2))
                .Append(transform.DOScale(defaultScale, animationSpeed / 2))
                .AppendCallback(() =>
                {
                    //Debug.Log(defaultScale);

                    behavior.enabled = true;

                    if (constraintsList.Count != 0)
                    {
                        for (int i = 0; i < constraintsList.Count; i++)
                            constraintsList[i].weight = 1;
                    }
                });
    }
}
