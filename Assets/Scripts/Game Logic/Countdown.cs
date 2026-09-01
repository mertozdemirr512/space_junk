using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Countdown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] private float _elapsedTime = 90f;
        void Update()
    {   
        if(GameManager.Instance.currentState != GameState.Gameplay)
        {
            return;
        }
        if(_elapsedTime > 0)
        {
            _elapsedTime -= Time.deltaTime;
            int seconds = Mathf.CeilToInt(_elapsedTime);
            timerText.text = string.Format("{0:0}",seconds);
        }
        else
        {
            _elapsedTime = 0;
            GameManager.Instance.OnLevelFail();
        }
    }
    
}
