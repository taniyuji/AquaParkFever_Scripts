using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//AddGateボタンクリック時の処理を追加するスクリプト
public class AddGateEventController : MonoBehaviour
{
    [SerializeField]
    private GateList gateList;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetFunctions);
    }

    //ゲートを増やす。ボタンの値段を上昇させる。ゲートの個数によってボタンの表示を変える。
    public void SetFunctions()
    {
        var informationManager = ResourceProvider.i.informationManager;

        informationManager.gateCount++;

        gateList.nowGateController.AddGate(informationManager.gateCount);
        ResourceProvider.i.moneyLimitController.IncreaseAddGateCost();
        ResourceProvider.i.UIController.SetMergePeopleButton();
        ResourceProvider.i.UIController.SetAddGateButton();
        //Debug.Log(mergeCount);
    }
}
