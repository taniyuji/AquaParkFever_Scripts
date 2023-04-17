using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//浮き輪の挙動を制御するスクリプト。
//Constraintだと微妙に位置がずれたり思うように回転しなかったりするのでスクリプトで制御する。
public class SwimRingBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform swimRing;

    private Vector3 defaultSwimRingPosition;

    void Start()
    {
        defaultSwimRingPosition = swimRing.transform.position;
    }


    public void SetSwimRingTransform(CharacterBehavior.AnimationNames state)
    {
        //キャラの状態によって浮き輪の位置や回転を調整する。
        if (!swimRing.gameObject.activeSelf) return;

        if (state == CharacterBehavior.AnimationNames.Climb)
        {
            swimRing.transform.position = swimRing.transform.position + new Vector3(0, 0.3f, 0);

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

    //浮き輪を出現させる処理
    public void SetSwimRing()
    {
        swimRing.gameObject.SetActive(true);

        defaultSwimRingPosition = swimRing.transform.localPosition;

        swimRing.transform.eulerAngles = new Vector3(10, 90, 0);

        swimRing.transform.position = swimRing.transform.position - new Vector3(0, -0.1f, 0);

        gameObject.tag = "2";
    }
}
