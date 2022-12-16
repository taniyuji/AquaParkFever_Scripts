using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageMover : MonoBehaviour
{
    [SerializeField]
    private float moveSpeedX;

    private Image image;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.AddPosX(Time.deltaTime * moveSpeedX);
    }
}
