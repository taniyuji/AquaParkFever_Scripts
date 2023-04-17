using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//デバッグ用のシーンリセット機能
public class ResetButton : MonoBehaviour
{
    [SerializeField]
    private int sceneNumber;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene(sceneNumber);
    }
}
