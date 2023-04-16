using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergePeopleEventController : MonoBehaviour
{
    [SerializeField]
    private MergePeopleBehavior mergePeopleBehavior;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetFunctions);
    }
    
    public void SetFunctions()
    {
        var informationManager = ResourceProvider.i.informationManager;
        if (informationManager.mergeCounter >= informationManager.peopleAmount - 1) return;

        informationManager.mergeCounter++;

        informationManager.peopleAmount--;

        mergePeopleBehavior.MergePeople();
        ResourceProvider.i.UIController.SetMergePeople();
        //Debug.Log(mergeCount);
    }
}
