using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

//スライダーに関係する各オブジェクトを提供するスクリプト
public class SliderResourceProvider : MonoBehaviour
{
    [SerializeField]
    private Transform baseTransform;

    public Transform getBaseTransform
    {
        get { return baseTransform; }
    }

    [SerializeField]
    private Transform climbLadderTransform;

    public Vector3 getClimbLadderPosition
    {
        get { return climbLadderTransform.position; }
    }
}
