using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using UniRx;
using System;

//スライダーの待機列を制御するスクリプト
public class WaitRow : MonoBehaviour
{
    [SerializeField]
    private float goIntervalTime;

    [SerializeField]
    private float defaultRowInterval = 0.4f;

    public float getDefaultRowInterval
    {
        get { return defaultRowInterval; }
    }

    private Subject<PathFollower> _goSlide = new Subject<PathFollower>();

    public IObservable<PathFollower> goSlide
    {
        get { return _goSlide; }
    }

    private List<PathFollower> _followerList = new List<PathFollower>();

    public bool isWaiting => _followerList.Count > 0;

    public float rowInterval { get; private set; }

    private float goIntervalCounter;

    // Update is called once per frame
    void Update()
    {
        //待機中のキャラがいない場合はここで返す。
        if (_followerList.Count <= 0) return;

        //一定間隔ごとにキャラを滑らせていく。
        if (goIntervalCounter < goIntervalTime)
        {
            goIntervalCounter += Time.deltaTime;
            return;
        }
        else
        {
            //滑らせる対象を通知
            _goSlide.OnNext(_followerList[0]);
            //滑らせる対象を待機列リストから削除
            _followerList.Remove(_followerList[0]);
            //待機列の長さを短くする。
            rowInterval -= defaultRowInterval;

            goIntervalCounter = 0;
        }
    }

    //キャラを待機列に追加する処理
    public void AddList(PathFollower follower)
    {
        if (_followerList.Contains(follower)) return;

        _followerList.Add(follower);

        StartCoroutine(setTargetTransform(follower));
    }

    public IEnumerator setTargetTransform(PathFollower follower)
    {
        yield return null;

        var setPosition = ResourceProvider.i.addSliderBehavior.nowSliderEntrancePosition;

        var sliderLeft = ResourceProvider.i.addSliderBehavior.nowBackSide;

        var fixedRowInterval = rowInterval * sliderLeft;

        follower.gameObject.transform.LookAt(setPosition);
        follower.gameObject.transform.position = setPosition - fixedRowInterval;

        rowInterval += defaultRowInterval;

        //Debug.Log(rowInterval);
    }

    //スライダーのレベルが上がり、足場の高さが変化した際にキャラを移動させる処理
    public void ChangeYPosition()
    {
        for (int g = 0; g < _followerList.Count; g++)
        {
            var position = _followerList[g].transform.position;
            _followerList[g].transform.position
             = new Vector3(position.x, ResourceProvider.i.addSliderBehavior.nowSliderEntrancePosition.y, position.z);

            _followerList[g].transform.localEulerAngles
             = ResourceProvider.i.peoplePool.peopleDefaultRotation;
            //Debug.Log("changeYPosition");
        }
    }
}
