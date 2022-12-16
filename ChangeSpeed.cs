using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpeedUp")) state = SpeedState.Up;
        else if (other.gameObject.CompareTag("SpeedDown")) state = SpeedState.Down;
    }
    // Start is called before the first frame update
    void Start()
    {
        state = SpeedState.Idle;

        defaultSpeed = follower.speed;

        follower = GetComponent<PathFollower>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == SpeedState.Idle) return;

        if(follower == null) follower = GetComponent<PathFollower>();

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
