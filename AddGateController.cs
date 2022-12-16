using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using System;

public class AddGateController : MonoBehaviour
{
    [SerializeField]
    private List<GateBehavior> gates;

    private AudioSource addGateSE;

    public int gateMaxAmount { get; private set; }

    void Awake()
    {
        addGateSE = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gates.Count; i++)
        {
            gates[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < ResourceProvider.i.informationManager.gateCount; i++)
        {
            gates[i].gameObject.SetActive(true);
        }

        ResourceProvider.i.informationManager.gateAmountChanged.Subscribe(i =>
        {
            if (i > gates.Count) return;

            gates[i - 1].gameObject.SetActive(true);

            if(addGateSE != null) addGateSE.PlayOneShot(addGateSE.clip);

            //Debug.Log("sliderAmount = " + i);
        });

        gateMaxAmount = gates.Count;
    }
}
