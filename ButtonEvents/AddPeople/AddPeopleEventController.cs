using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//AddPeopleボタンの処理を追加するスクリプト
public class AddPeopleEventController : MonoBehaviour
{
    [SerializeField]
    private AddPeopleBehavior addPeopleBehavior;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetFunctions);
    }

    //キャラを増やす。キャラを生成する値段を上昇。場に出ているキャラの人数によってボタンの表示を変化させる。
    public void SetFunctions()
    {
        var informationManager = ResourceProvider.i.informationManager;

        if (informationManager.peopleAmount
             >= informationManager.peopleAmountLimit) return;

        informationManager.peopleAmount++;

        addPeopleBehavior.AddPeople();
        ResourceProvider.i.moneyLimitController.IncreaseAddPeopleCost();
        ResourceProvider.i.UIController.SetAddPeople();
        ResourceProvider.i.UIController.SetMergePeople();

        //Debug.Log(peopleAmount);
    }
}
