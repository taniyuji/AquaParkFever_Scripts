using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

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

    public void AddDollar(int i)
    {
        if (i == 1) moneyAmount++;
        else moneyAmount += (i - 1) * 5;

        _moneyAmountChanged.OnNext(moneyAmount);
    }

    public void UseDollar(int i)
    {
        moneyAmount -= i;

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
