using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimRingBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform swimRing;

    private Vector3 defaultSwimRingPosition;
    // Start is called before the first frame update
    void Start()
    {
        defaultSwimRingPosition = swimRing.transform.position;
    }


    public void SetSwimRingTransform(CharacterBehavior.AnimationNames state)
    {
        if (!swimRing.gameObject.activeSelf) return;

        if (state == CharacterBehavior.AnimationNames.Climb)
        {
            swimRing.transform.position = new Vector3(swimRing.transform.position.x,
                                                      swimRing.transform.position.y + 0.3f,
                                                      swimRing.transform.position.z);

            swimRing.transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else if (state == CharacterBehavior.AnimationNames.OnSlider)
        {
            swimRing.transform.eulerAngles = new Vector3(90, 90, 0);

            swimRing.transform.localPosition = defaultSwimRingPosition;
        }
        else if (state == CharacterBehavior.AnimationNames.Swim)
        {
            swimRing.transform.eulerAngles = new Vector3(0, 90, 90);

            swimRing.transform.localPosition = defaultSwimRingPosition;
        }
        else
        {
            swimRing.transform.localPosition = defaultSwimRingPosition;

            swimRing.transform.eulerAngles = new Vector3(0, 90, 0);
        }
    }
    public void SetSwimRing(CharacterBehavior.AnimationNames state)
    {
        swimRing.gameObject.SetActive(true);

        defaultSwimRingPosition = swimRing.transform.localPosition;

        swimRing.transform.eulerAngles = new Vector3(10, 90, 0);

        swimRing.transform.position = new Vector3(swimRing.transform.position.x,
                                                  swimRing.transform.position.y - 0.1f,
                                                  swimRing.transform.position.z);

        gameObject.tag = "2";
    }
}
