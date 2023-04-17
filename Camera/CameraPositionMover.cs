using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//ゲーム中のカメラ移動を制御するスクリプト
public class CameraPositionMover : MonoBehaviour
{
    [SerializeField]
    private List<Transform> positionList;

    [SerializeField]
    private float cameraMoveSpeed = 0.5f;

    public void ChangePosition(int index)
    {
        if (index >= positionList.Count) return;

        //Debug.Log(i);

        transform.DOMove(positionList[index].position, cameraMoveSpeed);
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
