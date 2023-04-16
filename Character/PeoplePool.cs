using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//キャラのプーリングやスライダーを滑る効果音を管理するスクリプト
public class PeoplePool : MonoBehaviour
{
    [SerializeField]
    private AudioSource onSliderSE;

    [SerializeField]
    private int instantiatePeopleAmount;

    [SerializeField]
    private CharacterBehavior peoplePrefab;

    private List<CharacterBehavior> _peopleList = new List<CharacterBehavior>();

    public List<CharacterBehavior> peopleList
    {
        get { return _peopleList; }
    }

    void Awake()
    {
        peoplePrefab.gameObject.SetActive(false);

        _peopleList.Add(peoplePrefab);

        for (int i = 0; i < instantiatePeopleAmount; i++)
        {
            var instantiatedPeople = (Instantiate(peoplePrefab, transform));

            _peopleList.Add(instantiatedPeople);
        }
    }

    //AddPeopleで次に出現させるキャラを取得する。
    public CharacterBehavior GetNextPeople()
    {
        var target = _peopleList.FirstOrDefault(i => !i.gameObject.activeSelf);

        var nowSliderPosition = ResourceProvider.i.addSliderBehavior.nowSliderEntrancePosition;

        var rowInterval = ResourceProvider.i.waitRow.rowInterval;

        var sliderLeft = ResourceProvider.i.addSliderBehavior.nowBackSide;

        target.transform.LookAt(nowSliderPosition);

        var fixedRowInterval = sliderLeft * rowInterval;

        target.transform.position = nowSliderPosition - fixedRowInterval;

        target.gameObject.SetActive(true);

        //Debug.Log(target);
        return target;
    }

    //スライダー上を滑っているキャラが一人もいない場合、滑り効果音を停止する。
    public void CheckNoOneOnSlider()
    {
        if (!_peopleList.Any(i => i.state == CharacterBehavior.AnimationNames.OnSlider))
            onSliderSE.Pause();
    }

    public void playSE()
    {
        if (onSliderSE.isPlaying) return;

        onSliderSE.Play();
    }
}
