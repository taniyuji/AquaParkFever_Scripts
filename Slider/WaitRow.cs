using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using UniRx;
using System;

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
        if (_followerList.Count <= 0) return;

        if (goIntervalCounter < goIntervalTime)
        {
            goIntervalCounter += Time.deltaTime;
            return;
        }
        else
        {
            _goSlide.OnNext(_followerList[0]);

            _followerList.Remove(_followerList[0]);

            rowInterval -= defaultRowInterval;

            goIntervalCounter = 0;
        }
    }

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

    public void ChangeYPosition()
    {
        for (int g = 0; g < _followerList.Count; g++)
        {
            var position = _followerList[g].transform.position;
            _followerList[g].transform.position
             = new Vector3(position.x, ResourceProvider.i.addSliderBehavior.nowSliderEntrancePosition.y, position.z);
            //Debug.Log("changeYPosition");
        }
    }
}
