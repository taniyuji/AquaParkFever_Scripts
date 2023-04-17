using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ボタンクリック時のスケールを変化させるスクリプト
public class UIScaler : MonoBehaviour
{
    private RectTransform rectTransform;
    // Start is called before the first frame update

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetScaleCoroutine()
    {
        StartCoroutine(Scale());
    }

    private IEnumerator Scale()
    {
        transform.localScale = rectTransform.localScale - Vector3.one * 0.2f;

        yield return new WaitForSeconds(0.1f);

        transform.localScale = rectTransform.localScale + Vector3.one * 0.2f;
    }
}

