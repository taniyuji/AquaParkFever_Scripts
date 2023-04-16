using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using PathCreation;

//スライダーを切り替えるスクリプト
public class AddSliderBehavior : MonoBehaviour
{
    [SerializeField]
    private List<PathCreator> _sliderList;

    public List<PathCreator> sliderList { get { return _sliderList; } }

    private List<SliderResourceProvider> sliderResourceProviders = new List<SliderResourceProvider>();

    public Vector3 nowBaseBackPosition { get; private set; }

    public Vector3 nowClimbLadderPosition { get; private set; }

    public Vector3 nowSliderEntrancePosition { get; private set; }

    public Vector3 nowSliderJumpPosition { get; private set; }

    public PathCreator nowPath { get; private set; }

    public Vector3 nowBackSide { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < sliderList.Count; i++)
        {
            sliderResourceProviders.Add(sliderList[i].GetComponent<SliderResourceProvider>());
        }

        SetSlider(0);
    }

    public void SetSlider(int i)
    {
        if (i >= _sliderList.Count) return;

        //現在のスライダーのパス情報を更新
        nowPath = sliderList[i];

        if (i != 0) _sliderList[i - 1].gameObject.SetActive(false);

        _sliderList[i].gameObject.SetActive(true);

        var sliderResourceProvider = sliderResourceProviders[i];

        //スライダーの足場オブジェクトを活用してスライダーの方向や各動作の目印位置を取得する。
        var baseTransform = sliderResourceProvider.getBaseTransform;
        
        nowBackSide = -baseTransform.right;

        var nowForwardSide = -nowBackSide;

        var fixedBasePosition = new Vector3(baseTransform.position.x,
                                            baseTransform.position.y + baseTransform.localScale.y / 2,
                                            baseTransform.position.z);

        //梯子を登り終わるポジションを取得
        nowBaseBackPosition = fixedBasePosition + nowForwardSide * baseTransform.localScale.x / 2;
        //梯子の登る位置を取得
        nowClimbLadderPosition = sliderResourceProvider.getClimbLadderPosition;
        //スライダーの入り口の位置を取得
        nowSliderEntrancePosition = fixedBasePosition + nowBackSide * baseTransform.localScale.x / 4;

        //キャラがスライダーに向かって飛び込む位置を取得
        var fixedX = float.Parse(_sliderList[i].path.GetPoint(0).x.ToString("f2"));
        var fixedY = float.Parse(_sliderList[i].path.GetPoint(0).y.ToString("f2"));
        var fixedZ = float.Parse(_sliderList[i].path.GetPoint(0).z.ToString("f2"));
        nowSliderJumpPosition = new Vector3(fixedX, fixedY, fixedZ);
    }
}
