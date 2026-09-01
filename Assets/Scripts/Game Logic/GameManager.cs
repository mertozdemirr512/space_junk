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
    public static GameManager Instance{get; private set;}
    [SerializeField] private float scoreGoal;

    void Awake()
    {
        Instance = this;
    }

    [Header("Game State")]
    public GameState currentState;
    void Start()
    {
        GoalGate.score=0;
        ChangeGameState(GameState.Gameplay);
    }
    
    void ChangeGameState(GameState newState)
    {
        currentState = newState;

        switch(currentState)
        {
            case(GameState.Win):
            break;

            case(GameState.Loss):
            break;

            case(GameState.Gameplay):
            break;

            case(GameState.Menu):
            break;

        }
        
    }
    public void CheckScoreGoal()
    {
        if(currentState != GameState.Gameplay) return;

        if(GoalGate.score >= scoreGoal)
        {
            OnLevelWin();
        }
    }

    public void OnLevelFail()
    {
        Debug.Log("Level Failed");
        ChangeGameState(GameState.Loss);
    }

    public void OnLevelWin()
    {
        Debug.Log("Level Accomplished");
        ChangeGameState(GameState.Win);
    }


}
