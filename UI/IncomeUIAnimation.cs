using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Linq;

//ゲートを通った際のドルUIのアニメーションを制御するスクリプト
public class IncomeUIAnimation : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI incomeUI;

    [SerializeField]
    private int instantiateAmount;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float moveAmount;

    private Vector3 defaultPosition;

    private List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();

    private int index = 0;

    void Start()
    {
        defaultPosition = incomeUI.transform.position;

        for (int i = 0; i < instantiateAmount; i++)
        {
            var generatedObject = Instantiate(incomeUI, transform);

            generatedObject.gameObject.SetActive(false);

            textList.Add(generatedObject);
        }

        incomeUI.gameObject.SetActive(false);
    }

    public void MoveIncomeUI(int level)
    {
        var target = textList.Where(i => !i.gameObject.activeSelf).FirstOrDefault();

        if (target == null)
        {
            Debug.LogError("Need more incomeUI");

            return;
        } 

        target.gameObject.SetActive(true);

        if (level != 1) level = (level - 1) * 5;

        target.text = level + "$";

        var sequence = DOTween.Sequence();

        sequence.Append(target.transform.DOMoveY(target.transform.position.y + moveAmount, moveSpeed))
                .Append(target.transform.DOScale(new Vector3(0, 0, 0), moveSpeed))
        .AppendCallback(() =>
        {
            target.gameObject.SetActive(false);

            target.transform.position = defaultPosition;

            target.transform.localScale = Vector3.one;

            //Debug.Log("finishMoveIncomeUI");
        });
    }
}
