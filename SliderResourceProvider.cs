using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
