using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float _elapsedTime = 90f;

    [Header("Visual Effects")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.red;
    [SerializeField] private float warningThreshold = 10f; // Son kaç saniyede uyarı başlasın?

    private Vector3 originalScale;

    private void Start()
    {
        if (LevelManager.Instance != null && LevelManager.Instance.CurrentLevelData != null)
        {
            _elapsedTime = LevelManager.Instance.CurrentLevelData.timeLimit;
        }

        if (timerText != null)
        {
            originalScale = timerText.transform.localScale;
            timerText.color = normalColor;
        }
    }

    void Update()
    {   
        if (GameManager.Instance.currentState != GameState.Gameplay)
        {
            return;
        }

        if (_elapsedTime > 0)
        {
            _elapsedTime -= Time.deltaTime;
            int seconds = Mathf.CeilToInt(_elapsedTime);
            
            // "05" veya "90" formatında gösterim
            if (timerText != null)
            {
                timerText.text = string.Format("{0:00}", seconds);

                // Son 10 saniyede Kırmızı Renk ve Pulsing (Büyüyüp Küçülme)
                if (_elapsedTime <= warningThreshold)
                {
                    timerText.color = warningColor;
                    float scale = 1f + Mathf.PingPong(Time.time * 4f, 0.2f);
                    timerText.transform.localScale = originalScale * scale;
                }
                else
                {
                    timerText.color = normalColor;
                    timerText.transform.localScale = originalScale;
                }
            }
        }
        else
        {
            _elapsedTime = 0;
            if (timerText != null) timerText.text = "00";
            GameManager.Instance.OnLevelFail();
        }
    }
}