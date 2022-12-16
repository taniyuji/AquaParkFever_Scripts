using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PathCreation.Examples;

public class BeMergedAnimation : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private Sequence sequence;

    private PathFollower follower;

    private CharacterBehavior characterBehavior;

    void Awake()
    {
        follower = GetComponent<PathFollower>();

        characterBehavior = GetComponent<CharacterBehavior>();
    }


  
}
