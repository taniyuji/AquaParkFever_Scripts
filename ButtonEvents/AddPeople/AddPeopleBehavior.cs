using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using PathCreation.Examples;

//キャラクターの増減を管理するスクリプト
public class AddPeopleBehavior : MonoBehaviour
{

    [SerializeField]
    private Transform parentTransform;

    private float beforeAddTimer;

    private bool startCount = false;

    private AudioSource appearSE;

    private Vector3 peopleDefaultRotation;

    void Awake()
    {
        appearSE = GetComponent<AudioSource>();
    }
    
    //キャラを出現させる処理
    public void AddPeople()
    {
        appearSE.PlayOneShot(appearSE.clip);

        var InstantiatedCharacter = ResourceProvider.i.peoplePool.GetNextPeople();

        InstantiatedCharacter.transform.localEulerAngles
             = ResourceProvider.i.peoplePool.peopleDefaultRotation;

        //連打でキャラを生成しても一定の感覚を保たせる。
        //１つ前のキャラ生成から0.5秒経っていない場合は対象のキャラを待機状態にさせる。
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
    }

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
