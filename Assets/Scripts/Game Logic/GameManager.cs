using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Win,
    Loss,
    Gameplay,
    Menu
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("UI Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject lossPanel;

    [Header("Game State")]
    public GameState currentState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); //zaten var olan bir game manager varsa yenisini silsin
        }
    }

    private void Start()
    {
        GoalGate.score = 0;
        ChangeGameState(GameState.Gameplay);

     
        if (winPanel) winPanel.SetActive(false);
        if (lossPanel) lossPanel.SetActive(false);
    }
    
    public void ChangeGameState(GameState newState)
    {
        currentState = newState;

    switch(currentState)
        {
            case GameState.Win:
                Time.timeScale = 0f;
                if (winPanel != null) winPanel.SetActive(true);
                Debug.Log("Level Accomplished"); //
                break;

            case GameState.Loss:
                Time.timeScale = 0f;
                if (lossPanel != null) lossPanel.SetActive(true);
                Debug.Log("Level Failed");
                break;

            case GameState.Gameplay:
                Time.timeScale = 1f;
                break;

            case GameState.Menu:
                Time.timeScale = 1f;
                break;
            }
        }

    public void OnLevelFail()
    {
        ChangeGameState(GameState.Loss);
    }

    public void OnLevelWin()
    {
        ChangeGameState(GameState.Win);
    }
}