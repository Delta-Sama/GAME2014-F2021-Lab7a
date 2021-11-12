using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformBehaviour : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Transform destination;
    [SerializeField] private float moveTime = 0.5f;
    [SerializeField] private float stopDelay = 0.0f;

    private float localTime = 0.0f;
    private float currentDelay = 0.0f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 distance;

    void Start()
    {
        startPosition = transform.position;
        endPosition = destination.position;

        distance = (endPosition - startPosition);

    }

    void Update()
    {
        if (currentDelay > 0)
        {
            currentDelay -= Time.deltaTime;
            return;
        }

        MovePlatform();
    }

    private float prevPong = 0.0f;
    private float deltaPongSign = 1;

    private void MovePlatform()
    {
        localTime += Time.deltaTime / moveTime;

        float pong = Mathf.PingPong(localTime, 1.0f);
        float deltaPong = prevPong - pong;

        Vector3 move = Vector3.zero;
        if (distance.x != 0)
            move.x = pong * distance.x;
        if (distance.y != 0)
            move.y = pong * distance.y;

        // If direction changed, apply the delay
        if (deltaPongSign != Mathf.Sign(deltaPong))
        {
            deltaPongSign = Mathf.Sign(deltaPong);

            currentDelay = stopDelay;
        }

        transform.position = startPosition + new Vector3(move.x, move.y);

        prevPong = pong;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, destination.position);
    }
}
