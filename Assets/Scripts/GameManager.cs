using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static int Square = 0, Trail = 1;

    public Text goldText, diamondText;

    [HideInInspector] public bool gameStopped;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        goldText.text = PlayerPrefs.GetInt("Gold").ToString();
        diamondText.text = PlayerPrefs.GetInt("Diamond").ToString();
    }

    public void GameFail()
    {
        Debug.Log("Game Fail");
        gameStopped = true;
    }
}