using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class GateList : MonoBehaviour
{
    [SerializeField]
    private List<AddGateController> addGateControllers;

    [SerializeField]
    private int defaultGateLimit;

    public int nowGateMaxAmount { get; private set; }

    void Awake()
    {
        nowGateMaxAmount = defaultGateLimit;
    }
    void Start()
    {
        ResourceProvider.i.informationManager.SliderLevelChanged.Subscribe(i =>
        {
            StartCoroutine(GetMaxAmount(i));
        });
    }

    private IEnumerator GetMaxAmount(int i)
    {
        yield return null;

        nowGateMaxAmount = addGateControllers[i].gateMaxAmount;
    }
}
