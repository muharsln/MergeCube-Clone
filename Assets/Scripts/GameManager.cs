using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Square = 0, Trail = 1, Particle = 2;

    [HideInInspector]public int _goldAmount, _diamondAmount;

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
        _goldAmount = PlayerPrefs.GetInt("Gold");
        _diamondAmount = PlayerPrefs.GetInt("Diamond");
        goldText.text = PlayerPrefs.GetInt("Gold").ToString();
        diamondText.text = PlayerPrefs.GetInt("Diamond").ToString();
    }

    public void GameFail()
    {
        Debug.Log("Game Fail");
        gameStopped = true;
    }
}