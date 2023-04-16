using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

//AddSliderボタンの処理を追加するスクリプト
public class AddSliderEventController : MonoBehaviour
{
    [SerializeField]
    private AddSliderBehavior addSliderBehavior;

    [SerializeField]
    private GateList gateList;

    [SerializeField]
    private CameraPositionMover cameraPositionMover;

    private Subject<int> addSliderSubject = new Subject<int>();

    public IObservable<int> SliderLevelChanged
    {
        get { return addSliderSubject; }
    }

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetFunctions);
    }

    //カメラ位置、ゲートの個数、ボタンUIなど全てを再設定する。
    public void SetFunctions()
    {
        var informationManager = ResourceProvider.i.informationManager;

        informationManager.sliderLevel++;

        var sliderLevel = informationManager.sliderLevel;

        cameraPositionMover.ChangePosition(sliderLevel);
        gateList.ChangeNowGateMaxAmount(sliderLevel);
        addSliderBehavior.SetSlider(sliderLevel);
        ResourceProvider.i.waitRow.ChangeYPosition();
        ResourceProvider.i.moneyLimitController.IncreaseAddSliderCost();
        ResourceProvider.i.UIController.SetAddPeople();
        ResourceProvider.i.UIController.SetAddGate();
        ResourceProvider.i.UIController.SetAddSlider();

        addSliderSubject.OnNext(informationManager.sliderLevel);
        //Debug.Log(sliderLevel);

        informationManager.peopleAmountLimit += 7;

        //Debug.Log(peopleAmount);
    }
}
