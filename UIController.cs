using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI dollarUI;

    public enum FunctionName
    {
        AddPeople = 0,
        MergePeople = 1,
        AddSlider = 2,
        Income = 3,
    }

    [SerializeField]
    private List<Image> buttonImages;

    [SerializeField]
    private List<Image> subButtonImages;

    private List<Button> buttons = new List<Button>();

    private List<TextMeshProUGUI> buttonsTextList = new List<TextMeshProUGUI>();

    private List<Color> defaultImageColors = new List<Color>();

    private List<Color> defaultSubImageColors = new List<Color>();

    void Start()
    {
        SetButtonsList();

        StartCoroutine(SetDefaultUIs());

        UnSetButton(FunctionName.AddSlider);

        UnSetButton(FunctionName.MergePeople);

        UnSetButton(FunctionName.Income);

        ResourceProvider.i.informationManager.PeopleAmountChanged.Subscribe(i =>
        {
            SetCoroutine(FunctionName.AddPeople);

            SetCoroutine(FunctionName.MergePeople);
        });

        ResourceProvider.i.informationManager.SliderLevelChanged.Subscribe(i =>
        {
            SetCoroutine(FunctionName.AddPeople);

            SetCoroutine(FunctionName.Income);

            SetCoroutine(FunctionName.AddSlider);
        });

        ResourceProvider.i.informationManager.MergedPeople.Subscribe(i =>
        {
            SetCoroutine(FunctionName.MergePeople);
        });

        ResourceProvider.i.informationManager.gateAmountChanged.Subscribe(i =>
        {
            SetCoroutine(FunctionName.Income);
        });

        ResourceProvider.i.moneyLimitController.stateChanged.Subscribe(i =>
        {
            SetCoroutine(FunctionName.AddPeople);

            SetCoroutine(FunctionName.MergePeople);

            SetCoroutine(FunctionName.AddSlider);

            SetCoroutine(FunctionName.Income);
        });

        ResourceProvider.i.informationManager.moneyAmountChanged.Subscribe(i =>
        {
            dollarUI.text = i + "$";
        }
        );
    }

    private void SetButtonsList()
    {
        foreach (FunctionName value in Enum.GetValues(typeof(FunctionName)))
        {
            dollarUI.text = ResourceProvider.i.informationManager.moneyAmount + "$";

            var target = buttonImages[(int)value];

            defaultImageColors.Add(target.color);

            var subTarget = target.gameObject.transform.GetChild(1).GetComponent<Image>();

            subButtonImages.Add(subTarget);

            defaultSubImageColors.Add(subTarget.color);

            buttons.Add(buttonImages[(int)value].gameObject.GetComponent<Button>());

            buttonsTextList.Add(buttonImages[(int)value].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        }
    }

    private IEnumerator SetDefaultUIs()
    {
        yield return null;

        dollarUI.text = ResourceProvider.i.informationManager.moneyAmount + "$";

        buttonsTextList[(int)FunctionName.AddPeople].text
            = ResourceProvider.i.moneyLimitController.addPeopleLimit + "$";

        buttonsTextList[(int)FunctionName.MergePeople].text
               = ResourceProvider.i.moneyLimitController.mergePeopleLimit + "$";

        buttonsTextList[(int)FunctionName.AddSlider].text
               = ResourceProvider.i.moneyLimitController.addSliderLimit + "$";

        buttonsTextList[(int)FunctionName.Income].text
               = ResourceProvider.i.moneyLimitController.addGateLimit + "$";
    }

    public void UnSetButton(FunctionName name)
    {
        Color color = buttonImages[(int)name].color;

        var subColor = subButtonImages[(int)name].color;

        buttonImages[(int)name].color = new Color(color.r, color.g, color.b, color.a / 2);

        subButtonImages[(int)name].color = new Color(subColor.r, subColor.g, subColor.b, subColor.a / 2);

        buttons[(int)name].enabled = false;
    }

    public void SetButton(FunctionName name)
    {
        var color = defaultImageColors[(int)name];

        var subColor = defaultSubImageColors[(int)name];

        buttonImages[(int)name].color = new Color(color.r, color.g, color.b, color.a);

        subButtonImages[(int)name].color = new Color(subColor.r, subColor.g, subColor.b, subColor.a);

        buttons[(int)name].enabled = true;
    }

    private void SetCoroutine(FunctionName name)
    {
        IEnumerator method = SetAddPeople();

        switch (name)
        {
            case FunctionName.MergePeople: method = SetMergePeople(); break;
            case FunctionName.AddSlider: method = SetAddSlider(); break;
            case FunctionName.Income: method = SetAddGate(); break;
        }

        StartCoroutine(method);
    }

    private IEnumerator SetAddPeople()
    {
        yield return null;

        var informationManager = ResourceProvider.i.informationManager;

        var mergeCounter = informationManager.mergeCounter;

        var peopleAmount = informationManager.peopleAmount;

        if (peopleAmount >= ResourceProvider.i.informationManager.peopleAmountLimit
             || !ResourceProvider.i.moneyLimitController.canAddPeopleState)
        {
            if (buttons[(int)FunctionName.AddPeople].enabled)
                UnSetButton(FunctionName.AddPeople);
        }
        else
        {
            SetButton(FunctionName.AddPeople);
        }

        //Debug.Log("peopleAmount = " + peopleAmount + "peopleLimit = " + ResourceProvider.i.informationManager.peopleAmountLimit);

        if (peopleAmount >= ResourceProvider.i.informationManager.peopleAmountLimit)
        {
            buttonsTextList[(int)FunctionName.AddPeople].text = "Max";
        }
        else
        {
            buttonsTextList[(int)FunctionName.AddPeople].text
             = ResourceProvider.i.moneyLimitController.addPeopleLimit + "$";
        }
    }

    private IEnumerator SetMergePeople()
    {
        yield return null;

        var mergeCounter = ResourceProvider.i.informationManager.mergeCounter;

        var peopleAmount = ResourceProvider.i.informationManager.peopleAmount;

        if (peopleAmount - 1 <= mergeCounter ||
                    !ResourceProvider.i.moneyLimitController.canMergePeopleState)
        {
            if (buttons[(int)FunctionName.MergePeople].enabled)
                UnSetButton(FunctionName.MergePeople);
        }
        else
        {
            SetButton(FunctionName.MergePeople);
        }

        if (peopleAmount - 1 <= mergeCounter)
        {
            buttonsTextList[(int)FunctionName.MergePeople].text = "Max";
        }
        else
        {
            buttonsTextList[(int)FunctionName.MergePeople].text
                = ResourceProvider.i.moneyLimitController.mergePeopleLimit + "$";
        }
    }

    private IEnumerator SetAddSlider()
    {
        yield return null;

        var sliderLevel = ResourceProvider.i.informationManager.sliderLevel;

        var sliderMaxLevel = ResourceProvider.i.addSliderBehavior.sliderList.Count - 1;

        if (!ResourceProvider.i.moneyLimitController.canAddSliderState
                                         || sliderLevel == sliderMaxLevel)
        {
            if (buttons[(int)FunctionName.AddSlider].enabled)
                UnSetButton(FunctionName.AddSlider);
        }
        else
        {
            SetButton(FunctionName.AddSlider);
        }

        if (sliderLevel == sliderMaxLevel)
        {
            buttonsTextList[(int)FunctionName.AddSlider].text = "Max";
        }
        else
        {
            buttonsTextList[(int)FunctionName.AddSlider].text
             = ResourceProvider.i.moneyLimitController.addSliderLimit + "$";
        }
    }

    private IEnumerator SetAddGate()
    {
        yield return null;

        var gateAmount = ResourceProvider.i.informationManager.gateCount;

        var nowMaxGateAmount = ResourceProvider.i.gateList.nowGateMaxAmount;

        if (!ResourceProvider.i.moneyLimitController.canAddGateState
                                          || gateAmount >= nowMaxGateAmount)
        {
            if (buttons[(int)FunctionName.Income].enabled)
                UnSetButton(FunctionName.Income);
        }
        else
        {
            SetButton(FunctionName.Income);
        }

        if (gateAmount >= nowMaxGateAmount)
        {
            buttonsTextList[(int)FunctionName.Income].text = "Max";

            //Debug.Log("now Gate Max" + nowMaxGateAmount);
        }
        else
        {
            buttonsTextList[(int)FunctionName.Income].text
             = ResourceProvider.i.moneyLimitController.addGateLimit + "$";
        }
    }
}
