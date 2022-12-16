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
    private List<Vector3> positionList;

    void Start()
    {
        ResourceProvider.i.informationManager.SliderLevelChanged.Subscribe(i =>
        {
            if (i >= positionList.Count) return;

            Debug.Log(i);

            transform.position = positionList[i];
        });
    }

    public Vector3 GetPosition(int index)
    {
        return positionList[index];
    }
}
