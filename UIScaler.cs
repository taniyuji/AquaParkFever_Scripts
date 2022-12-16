using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        transform.localScale = new Vector3(rectTransform.localScale.x - 0.2f,
                                           rectTransform.localScale.y - 0.2f,
                                           rectTransform.localScale.z - 0.2f);

        yield return new WaitForSeconds(0.1f);

        transform.localScale = new Vector3(rectTransform.localScale.x + 0.2f,
                                           rectTransform.localScale.y + 0.2f,
                                           rectTransform.localScale.z + 0.2f);
    }
}

