using System.Collections.Generic;
using UnityEngine;

//スライダーのゲートの増減を管理するスクリプト。スライダーオブジェクトに添付。
public class AddGateController : MonoBehaviour
{
    [SerializeField]
    private Transform gatesParent;

    private List<GateAnimation> gates = new List<GateAnimation>();

    private AudioSource addGateSE;

    public int gateMaxAmount { get; private set; }

    //スライダーオブジェクト自体のアクティブがfalseでゲート情報が取得できなくならないよう
    //あらかじめ外部スクリプトからゲート情報を取得するSetInformation関数を走らせる。
    public void SetInformation()
    {
        addGateSE = GetComponent<AudioSource>();

        gates.AddRange(gatesParent.GetComponentsInChildren<GateAnimation>());
        gateMaxAmount = gates.Count;
        //Debug.Log(gameObject.transform.root.name + "getGates");
    }

    //ゲートオブジェクトを増やす処理
    public void AddGate(int i)
    {
        if (i > gates.Count) return;

        gates[i - 1].gameObject.SetActive(true);

        if (addGateSE != null) addGateSE.PlayOneShot(addGateSE.clip);

        //Debug.Log("sliderAmount = " + i);
    }

    void Start()
    {

        for (int i = 0; i < gates.Count; i++)
        {
            gates[i].gameObject.SetActive(false);
        }

        //前のスライダーで解放済みだったゲートの個数を引き継ぐ
        for (int i = 0; i < ResourceProvider.i.informationManager.gateCount; i++)
        {
            gates[i].gameObject.SetActive(true);
        }

        gateMaxAmount = gates.Count;
    }
}
