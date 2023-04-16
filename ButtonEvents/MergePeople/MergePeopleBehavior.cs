using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using System;

//キャラのマージを管理するスクリプト
public class MergePeopleBehavior : MonoBehaviour
{
    public void MergePeople()
    {
        var peoplePool = ResourceProvider.i.peoplePool;

        //先頭からレベルがまだ初期状態のキャラを取得する。
        var target = peoplePool.peopleList.Where(i => i.gameObject.CompareTag("1"))
                                                .FirstOrDefault()
                                                .GetComponent<CharacterBehavior>();

        if (target != null)
        {
            target.SetMerge(false);

            var mergedPeople = peoplePool.peopleList.Where(i => i.gameObject.activeSelf)
                                        .LastOrDefault()
                                        .GetComponent<CharacterBehavior>();

            mergedPeople.SetMerge(true);
        }
    }
}
