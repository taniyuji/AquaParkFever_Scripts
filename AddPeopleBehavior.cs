using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using PathCreation.Examples;

public class AddPeopleBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform parentTransform;

    private float beforeAddTimer;

    private bool startCount = false;

    private AudioSource appearSE;

    void Awake()
    {
        appearSE = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResourceProvider.i.informationManager.PeopleAmountChanged.Subscribe(i =>
        {
            appearSE.PlayOneShot(appearSE.clip);
            
            var InstantiatedCharacter = ResourceProvider.i.mergePeopleBehavior.GetNextPeople();

            if (beforeAddTimer < 0.5f && startCount)
            {
                InstantiatedCharacter.SetRowWaiting();

                var follower = InstantiatedCharacter.gameObject.GetComponent<PathFollower>();
                follower.enabled = false;
                ResourceProvider.i.waitRow.AddList(follower);
                //Debug.Log("AddToList");
            }

            startCount = true;
            beforeAddTimer = 0;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (startCount)
        {
            if (beforeAddTimer >= 0.5f)
            {
                startCount = false;
                return;
            }

            beforeAddTimer += Time.deltaTime;
        }
    }
}
