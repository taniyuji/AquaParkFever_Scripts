using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using System;

public class MergePeopleBehavior : MonoBehaviour
{
    [SerializeField]
    private AudioSource onSliderSE;

    [SerializeField]
    private int instantiatePeopleAmount;

    [SerializeField]
    private List<CharacterBehavior> peoplePrefabs;

    private List<CharacterBehavior> peopleList = new List<CharacterBehavior>();

    private int index = 0;


    void OnEnable()
    {
        for (int i = 0; i < peoplePrefabs.Count; i++)
        {
            peopleList.Add(peoplePrefabs[i]);

            if (i != 0) peoplePrefabs[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < instantiatePeopleAmount; i++)
        {
            var instantiatedPeople = (Instantiate(peoplePrefabs[index], transform));

            instantiatedPeople.gameObject.SetActive(false);

            peopleList.Add(instantiatedPeople);

            index = index == peoplePrefabs.Count - 1 ? index = 0 : index += 1;
        }
    }

    void Start()
    {
        ResourceProvider.i.informationManager.MergedPeople.Subscribe(i =>
        {
            var target = peopleList.Where(i => i.gameObject.CompareTag("1"))
                                                .FirstOrDefault()
                                                .GetComponent<CharacterBehavior>();

            if (target != null)
            {
                target.SetMerge(false);

                var mergedPeople = peopleList.Where(i => i.gameObject.activeSelf)
                                            .LastOrDefault()
                                            .GetComponent<CharacterBehavior>();

                mergedPeople.SetMerge(true);
            }
        });
    }

    public CharacterBehavior GetNextPeople()
    {
        var target = peopleList.FirstOrDefault(i => !i.gameObject.activeSelf);

        var nowSliderPosition = ResourceProvider.i.addSliderBehavior.nowSliderEntrancePosition;

        var rowInterval = ResourceProvider.i.waitRow.rowInterval;

        var sliderLeft = ResourceProvider.i.addSliderBehavior.nowLeftSide;

        target.transform.LookAt(nowSliderPosition);

        var fixedRowInterval = sliderLeft * rowInterval;

        target.transform.position = nowSliderPosition - fixedRowInterval;

        target.gameObject.SetActive(true);

        //Debug.Log(target);
        return target;
    }

    public void CheckNoOneOnSlider()
    {
        if (!peopleList.Any(i => i.state == CharacterBehavior.AnimationNames.OnSlider))
            onSliderSE.Pause();
    }

    public void playSE()
    {
        if (onSliderSE.isPlaying) return;

        onSliderSE.Play();
    }
}
