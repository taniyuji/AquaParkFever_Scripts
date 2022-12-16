using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class InformationManager : MonoBehaviour
{
    [SerializeField]
    private bool resetMoney = false;

    [SerializeField]
    private Transform _poolLadderTransform;

    [SerializeField]
    private int defaultMoneyAmount;

    [SerializeField]
    private int defaultPeopleLimit;

    [SerializeField]
    private int defaultGateAmount;

    public Transform poolLadderTransform
    {
        get { return _poolLadderTransform; }
    }

    [SerializeField]
    private Transform _climbLadderTransform;

    public Transform climbLadderTransform
    {
        get { return _climbLadderTransform; }
    }

    private Subject<int> addPeopleSubject = new Subject<int>();

    public IObservable<int> PeopleAmountChanged
    {
        get { return addPeopleSubject; }
    }

    public int peopleAmount { get; private set; } = 1;

    public int peopleAmountLimit { get; private set; } = 7;

    private Subject<int> mergePeopleSubject = new Subject<int>();

    public IObservable<int> MergedPeople
    {
        get { return mergePeopleSubject; }
    }

    public int mergeCounter { get; private set; } = 0;

    private Subject<int> addSliderSubject = new Subject<int>();

    public IObservable<int> SliderLevelChanged
    {
        get { return addSliderSubject; }
    }

    public int sliderLevel { get; private set; } = 0;

    private Subject<int> _gateAmountChanged = new Subject<int>();

    public IObservable<int> gateAmountChanged
    {
        get { return _gateAmountChanged; }
    }

    public int gateCount { get; private set; } = 0;

    private Subject<int> _moneyAmountChanged = new Subject<int>();

    public IObservable<int> moneyAmountChanged
    {
        get { return _moneyAmountChanged; }
    }

    public int moneyAmount { get; private set; }

    public int beforeMaterialIndex = 0;

    void Awake()
    {
        moneyAmount = defaultMoneyAmount;

        peopleAmountLimit += defaultPeopleLimit;

        gateCount += defaultGateAmount;
    }

    public void AddPeople()
    {
        if (peopleAmount >= peopleAmountLimit) return;

        peopleAmount++;

        addPeopleSubject.OnNext(peopleAmount);

        //Debug.Log(peopleAmount);
    }

    public void MergePeople()
    {
        if (mergeCounter >= peopleAmount - 1) return;

        mergeCounter++;

        peopleAmount--;

        mergePeopleSubject.OnNext(mergeCounter);

        //Debug.Log(mergeCount);
    }

    public void AddSliderLevel()
    {
        sliderLevel++;

        addSliderSubject.OnNext(sliderLevel);

        peopleAmountLimit += 7;
    }

    public void AddGate()
    {
        gateCount++;

        _gateAmountChanged.OnNext(gateCount);
        //Debug.Log(gateCount);
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
