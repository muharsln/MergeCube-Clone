using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public bool _gameStopped;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void GameFail()
    {
        Debug.Log("Game Fail");
        _gameStopped = true;
    }



}
