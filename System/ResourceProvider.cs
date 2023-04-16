using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ResourceProvider : MonoBehaviour
{
    public static ResourceProvider i { get; private set; }

    [SerializeField]
    private InformationProvider _informationManager;

    public InformationProvider informationManager
    {
        get { return _informationManager; }
    }

    [SerializeField]
    private AddSliderBehavior _addSliderBehavior;

    public AddSliderBehavior addSliderBehavior
    {
        get { return _addSliderBehavior; }
    }

    [SerializeField]
    private WaitRow _waitRow;

    public WaitRow waitRow
    {
        get { return _waitRow; }
    }

    [SerializeField]
    private PeoplePool _peoplePool;

    public PeoplePool peoplePool
    {
        get { return _peoplePool; }
    }

    [SerializeField]
    private MoneyLimitController _moneyLimitController;

    public MoneyLimitController moneyLimitController
    {
        get { return _moneyLimitController; }
    }

    [SerializeField]
    private UIController _UIController;

    public UIController UIController
    {
        get { return _UIController; }
    }

    [SerializeField]
    private AddSliderEventController _addSliderEventController;

    public AddSliderEventController addSliderEventController
    {
        get { return _addSliderEventController; }
    }

    [SerializeField]
    private List<Transform> _mergeTransform;

    public List<Transform> mergeTransform
    {
        get { return _mergeTransform; }
    }

    [SerializeField]
    private Transform _poolLadderTransform;

    public Transform poolLadderTransform
    {
        get { return _poolLadderTransform; }
    }

    void Awake()
    {
        if (i == null) i = this;
    }
}
