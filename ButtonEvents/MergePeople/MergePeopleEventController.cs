using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//MergePeopleボタンに処理を追加するスクリプト
public class MergePeopleEventController : MonoBehaviour
{
    [SerializeField]
    private MergePeopleBehavior mergePeopleBehavior;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetFunctions);
    }

    //キャラをマージ。MergePeopleのコストを上昇。ボタンの状態を変更。
    public void SetFunctions()
    {
        var informationManager = ResourceProvider.i.informationManager;
        if (informationManager.mergeCounter >= informationManager.peopleAmount - 1) return;

        informationManager.mergeCounter++;

        informationManager.peopleAmount--;

        mergePeopleBehavior.MergePeople();
        ResourceProvider.i.moneyLimitController.IncreaseMergePeopleCost();
        ResourceProvider.i.UIController.SetMergePeopleButton();
        //Debug.Log(mergeCount);
    }
}
