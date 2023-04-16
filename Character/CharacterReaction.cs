using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterReaction : MonoBehaviour
{
    [SerializeField]
    private Canvas reactionCanvas;

    [SerializeField]
    private float startWaitTime;

    [SerializeField]
    private float scaleAmount;

    [SerializeField]
    private float animationTime;

    [SerializeField]
    private MergePeopleBehavior mergePeopleBehavior;

    private RectTransform rectTransform;

    private RectTransform defaultTransform;


    void Awake()
    {
        rectTransform = reactionCanvas.GetComponent<RectTransform>();

        reactionCanvas.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlaySequence();
    }

    private void PlaySequence()
    {
        StartCoroutine(setCanvas(startWaitTime));

        var sequence = DOTween.Sequence();

        var scale = rectTransform.localScale;

        var scaleVector = new Vector3(scaleAmount, scaleAmount, scaleAmount);

        sequence.Append(rectTransform.DOScale(scale + scaleVector, animationTime / 4).SetDelay(startWaitTime))
                .Append(rectTransform.DOScale(scale, animationTime / 4))
                .Append(rectTransform.DOScale(scale + scaleVector, animationTime / 4))
                .Append(rectTransform.DOScale(scale, animationTime / 4))
                .AppendCallback(() =>
                {
                    reactionCanvas.gameObject.SetActive(false);
                });
    }

    private IEnumerator setCanvas(float sec)
    {
        reactionCanvas.transform.parent = mergePeopleBehavior.transform;

        yield return new WaitForSeconds(sec);

        reactionCanvas.gameObject.SetActive(true);
    }
}
