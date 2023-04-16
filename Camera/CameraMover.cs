using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMover : MonoBehaviour
{
    [SerializeField]
    private List<Transform> cameraTransformList;

    [SerializeField]
    private float moveSpeed;

    private bool isMoving = false;

    private Sequence sequence;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            moveSequence();
        }
    }

    private void moveSequence()
    {
        isMoving = true;

        sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(cameraTransformList[0].position, moveSpeed))
                .Join(transform.DOLocalRotate(cameraTransformList[0].localEulerAngles, moveSpeed))
                .AppendCallback(() => { isMoving = false; });
    }
}
