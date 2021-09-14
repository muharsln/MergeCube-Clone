using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public bool _gameStopped;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void GameFail()
    {
        Debug.Log("Game Fail");
        _gameStopped = true;
    }
}