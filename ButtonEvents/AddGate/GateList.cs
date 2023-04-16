using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

//各ゲート管理スクリプトを管理するスクリプト。
//現在のスライダーにおけるゲートの最大個数や現在のスライダーのゲートを制御しているコンポーネントを提供する。
public class GateList : MonoBehaviour
{
    [SerializeField]
    private List<AddGateController> addGateControllers;

    public int nowGateMaxAmount { get; private set; }
    public AddGateController nowGateController { get; private set; }

    void Awake()
    {
        //アクティブがfalseのスライダーでもゲート情報を取得できるよう
        //あらかじめゲート情報を取得するSetInformation関数を起動する。
        for (int i = 0; i < addGateControllers.Count; i++)
        {
            addGateControllers[i].SetInformation();
            //Debug.Log(addGateControllers[i].gateMaxAmount);
        }
    }

    //スライダーのレベルが上がった際に次のスライダーのゲート最大個数と管理スクリプトを取得する
    public void ChangeNowGateMaxAmount(int index)
    {
        nowGateMaxAmount = addGateControllers[index].gateMaxAmount;
        nowGateController = addGateControllers[index];
    }

    void Start()
    {
        nowGateMaxAmount = addGateControllers[0].gateMaxAmount;
        nowGateController = addGateControllers[0];
    }
}
