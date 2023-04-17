using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

//ゲームの各情報を管理するスクリプト
public class InformationProvider : MonoBehaviour
{
    [SerializeField]
    private bool resetMoney = false;

    [SerializeField]
    private int defaultMoneyAmount;

    [SerializeField]
    private int defaultPeopleLimit;

    [SerializeField]
    private int defaultGateAmount;

    [HideInInspector]
    public int peopleAmount = 1;

    [HideInInspector]
    public int peopleAmountLimit = 7;

    [HideInInspector]
    public int mergeCounter = 0;

    [HideInInspector]
    public int sliderLevel = 0;

    [HideInInspector]
    public int gateCount = 0;

    [HideInInspector]
    public int moneyAmount;

    private Subject<int> _moneyAmountChanged = new Subject<int>();

    public IObservable<int> moneyAmountChanged
    {
        get { return _moneyAmountChanged; }
    }

    public int beforeMaterialIndex = 0;

    void Awake()
    {
        moneyAmount = defaultMoneyAmount;

        peopleAmountLimit += defaultPeopleLimit;

        gateCount += defaultGateAmount;
    }

    //お金を増やす処理。引数で渡されるキャラのレベルによって増やす値段を変える。
    public void AddDollar(int i)
    {
        if (i == 1) moneyAmount++;
        else moneyAmount += (i - 1) * 5;
        //所持金額が増えたことを通知
        _moneyAmountChanged.OnNext(moneyAmount);
    }

    //お金を使う処理。引数で渡された額を引く。
    public void UseDollar(int i)
    {
        moneyAmount -= i;
        //所持金額が減ったことを通知
        _moneyAmountChanged.OnNext(moneyAmount);
    }

    void Update()
    {
        if (resetMoney)
        {
            moneyAmount = 200;

            resetMoney = false;
        }


    }
}
