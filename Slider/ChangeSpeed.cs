using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

//キャラの滑るスピードを変化させるスクリプト
public class ChangeSpeed : MonoBehaviour
{
    [SerializeField]
    private float speedUpAmount;

    [SerializeField]
    private float speedDownAmount;

    private PathFollower follower;

    private float defaultSpeed;

    private enum SpeedState
    {
        Idle,
        Up,
        Down,
    }

    private SpeedState state;

    //スピードを変化させるコライダーにヒットすると速度が変化
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpeedUp")) state = SpeedState.Up;
        else if (other.gameObject.CompareTag("SpeedDown")) state = SpeedState.Down;
    }

    void Start()
    {
        state = SpeedState.Idle;

        follower = GetComponent<PathFollower>();

        defaultSpeed = follower.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == SpeedState.Idle) return;

        if(follower == null) follower = GetComponent<PathFollower>();

        //素の速度に戻ったら速度変化処理を終了する
        if (follower.speed < defaultSpeed)
        {
            follower.speed = defaultSpeed;

            state = SpeedState.Idle;

            return;
        }

        follower.speed = state == SpeedState.Up
             ? follower.speed += speedUpAmount * Time.deltaTime : follower.speed -= speedDownAmount * Time.deltaTime;

       // Debug.Log(follower.speed);

    }
}
