using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ResourceProvider : MonoBehaviour
{
    public static ResourceProvider i { get; private set; }

    [SerializeField]
    private CameraPositionMover _cameraPositionMover;

    public CameraPositionMover cameraPositionMover
    {
        get { return _cameraPositionMover; }
    }

    [SerializeField]
    private InformationManager _informationManager;

    public InformationManager informationManager
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
    private CharacterBehavior _characterBehavior;

    public CharacterBehavior characterBehavior
    {
        get { return _characterBehavior; }
    }

    [SerializeField]
    private WaitRow _waitRow;

    public WaitRow waitRow
    {
        get { return _waitRow; }
    }

    [SerializeField]
    private MergePeopleBehavior _mergePeopleBehavior;

    public MergePeopleBehavior mergePeopleBehavior
    {
        get { return _mergePeopleBehavior; }
    }

    [SerializeField]
    private GateList _gateList;

    public GateList gateList
    {
        get { return _gateList; }
    }

    [SerializeField]
    private MoneyLimitController _moneyLimitController;

    public MoneyLimitController moneyLimitController
    {
        get { return _moneyLimitController; }
    }

    [SerializeField]
    private List<Transform> _mergeTransform;

    public List<Transform> mergeTransform
    {
        get { return _mergeTransform; }
    }

    void Awake()
    {
        if (i == null) i = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
