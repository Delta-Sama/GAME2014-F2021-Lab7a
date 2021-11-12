using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject Player;
    public Transform spawnPointTransform;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ResetPlayer();
    }

    public void ResetPlayer()
    {
        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        Player.transform.position = spawnPointTransform.position;
    }
}
