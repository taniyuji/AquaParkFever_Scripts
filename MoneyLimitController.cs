using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class MoneyLimitController : MonoBehaviour
{
    private Subject<bool> _stateChanged = new Subject<bool>();

    public IObservable<bool> stateChanged
    {
        get { return _stateChanged; }
    }

    [SerializeField]
    private int addPeopleCost;

    public int addPeopleLimit { get; private set; }

    public bool canAddPeopleState { get; private set; }


    public int mergePeopleLimit { get; private set; }

    [SerializeField]
    private int mergePeopleCost;

    public bool canMergePeopleState { get; private set; }


    public int addSliderLimit { get; private set; }

    [SerializeField]
    private int addSliderCost;

    public bool canAddSliderState { get; private set; }
    

    public int addGateLimit { get; private set; }

    [SerializeField]
    private int addGateCost;

    public bool canAddGateState { get; private set; }

    // Start is called before the first frame update

    void Awake()
    {
        addPeopleLimit = addPeopleCost;

        mergePeopleLimit = mergePeopleCost;

        addSliderLimit = addSliderCost;

        addGateLimit = addGateCost;
    }

    void Start()
    {
        ResourceProvider.i.informationManager.moneyAmountChanged.Subscribe(i =>
        {
            canAddPeopleState = i >= addPeopleLimit;

            canMergePeopleState = i >= mergePeopleLimit;

            canAddSliderState = i >= addSliderLimit;

            canAddGateState = i >= addGateLimit;

            _stateChanged.OnNext(true);
        });

        ResourceProvider.i.informationManager.PeopleAmountChanged.Subscribe(i =>
        {
            ResourceProvider.i.informationManager.UseDollar(addPeopleLimit);

            addPeopleLimit += addPeopleCost;

            if (ResourceProvider.i.informationManager.moneyAmount < addPeopleLimit)
                canAddPeopleState = false;
        });


        ResourceProvider.i.informationManager.MergedPeople.Subscribe(i =>
        {
            ResourceProvider.i.informationManager.UseDollar(mergePeopleLimit);

            mergePeopleLimit += mergePeopleCost;

            if (ResourceProvider.i.informationManager.moneyAmount < mergePeopleLimit)
                canMergePeopleState = false;
        });


        ResourceProvider.i.informationManager.SliderLevelChanged.Subscribe(i =>
        {
            ResourceProvider.i.informationManager.UseDollar(addSliderLimit);

            addSliderLimit += addSliderCost;

            if (ResourceProvider.i.informationManager.moneyAmount < addSliderLimit)
                canAddSliderState = false;
        });


        ResourceProvider.i.informationManager.gateAmountChanged.Subscribe(i =>
        {
            ResourceProvider.i.informationManager.UseDollar(addGateLimit);

            addGateLimit += addGateCost;

            if (ResourceProvider.i.informationManager.moneyAmount < addGateLimit)
                canAddGateState = false;
        });
    }
}
