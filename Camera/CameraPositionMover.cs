using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEditor;
using PathCreation.Examples;
using PathCreation;
using System.Linq;
public class CameraPositionMover : MonoBehaviour
{
    [SerializeField]
    private List<Transform> positionList;

    public void ChangePosition(int index)
    {
        if (index >= positionList.Count) return;

        //Debug.Log(i);

        transform.position = positionList[index].position;
    }

    void Start()
    {
        transform.position = positionList[0].position;
    }

    public Vector3 GetPosition(int index)
    {
        return positionList[index].position;
    }
}
