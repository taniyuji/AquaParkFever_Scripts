using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using PathCreation;

public class AddSliderBehavior : MonoBehaviour
{
    [SerializeField]
    private List<PathCreator> _sliderList;

    public List<PathCreator> sliderList { get { return _sliderList; } }

    [SerializeField]
    private SliderResourceProvider firstSliderResourceProvider;

    public Vector3 nowBaseBackPosition { get; private set; }

    public Vector3 nowClimbLadderPosition { get; private set; }

    public Vector3 nowSliderEntrancePosition { get; private set; }

    public Vector3 nowSliderJumpPosition { get; private set; }

    public PathCreator nowPath { get; private set; }

    public Vector3 nowLeftSide { get; private set; }

    private SliderResourceProvider nextSliderResourceProvider;

    // Start is called before the first frame update
    void Awake()
    {
        nowPath = sliderList[0];

        var sliderResourceProvider = firstSliderResourceProvider;

        var baseTransform = sliderResourceProvider.getBaseTransform;

        nowLeftSide = -baseTransform.right;

        var nowRightSide = -nowLeftSide;

        var fixedBasePosition = new Vector3(baseTransform.position.x,
                                            baseTransform.position.y + baseTransform.localScale.y / 2,
                                            baseTransform.position.z);

        nowBaseBackPosition = fixedBasePosition + nowRightSide * baseTransform.localScale.x / 2;

        nowClimbLadderPosition = sliderResourceProvider.getClimbLadderPosition;

        nowSliderEntrancePosition = fixedBasePosition + nowLeftSide * baseTransform.localScale.x / 4;


        var fixedX = float.Parse(_sliderList[0].path.GetPoint(0).x.ToString("f2"));

        var fixedY = float.Parse(_sliderList[0].path.GetPoint(0).y.ToString("f2"));

        var fixedZ = float.Parse(_sliderList[0].path.GetPoint(0).z.ToString("f2"));

        nowSliderJumpPosition = new Vector3(fixedX, fixedY, fixedZ);

        nextSliderResourceProvider = _sliderList[1].GetComponent<SliderResourceProvider>();
    }

    void Start()
    {
        ResourceProvider.i.informationManager.SliderLevelChanged.Subscribe(i =>
        {
            if (i >= _sliderList.Count) return;

            _sliderList[i - 1].gameObject.SetActive(false);

            nowPath = _sliderList[i];

            nowPath.gameObject.SetActive(true);

            var sliderResourceProvider = nextSliderResourceProvider;

            var baseTransform = sliderResourceProvider.getBaseTransform;

            nowLeftSide = -baseTransform.right;

            var nowRightSide = -nowLeftSide;

            var fixedBasePosition = new Vector3(baseTransform.position.x,
                                                baseTransform.position.y + baseTransform.localScale.y / 2,
                                                baseTransform.position.z);

            nowBaseBackPosition = fixedBasePosition + nowRightSide * baseTransform.localScale.x / 2;

            nowClimbLadderPosition = sliderResourceProvider.getClimbLadderPosition;

            nowSliderEntrancePosition = fixedBasePosition + nowLeftSide * baseTransform.localScale.x / 4;

            var fixedX = float.Parse(_sliderList[i].path.GetPoint(0).x.ToString("f2"));

            var fixedY = float.Parse(_sliderList[i].path.GetPoint(0).y.ToString("f2"));

            var fixedZ = float.Parse(_sliderList[i].path.GetPoint(0).z.ToString("f2"));

            nowSliderJumpPosition = new Vector3(fixedX, fixedY, fixedZ);

            nextSliderResourceProvider = _sliderList[i + 1].GetComponent<SliderResourceProvider>();
        });
    }
}
