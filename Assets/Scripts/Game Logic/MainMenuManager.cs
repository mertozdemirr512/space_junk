using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject levelSelectPanel;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text playButtonText; // Buton üzerinde "DEVAM ET (LEVEL 3)" yazması için

    private int lastUnlockedLevel = 1;

    private void Start()
    {
        // Oyuncunun kaldığı son seviyeyi çek (Varsayılan: 1)
        lastUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        // Başlat butonunun üzerindeki yazıyı dinamik yap
        if (playButtonText != null)
        {
            playButtonText.text = lastUnlockedLevel == 1 ? "START" : $"CONTINUE (LEVEL {lastUnlockedLevel})";
        }

        ShowMainPanel();
    }

    public void ShowMainPanel()
    {
        if (mainPanel != null) mainPanel.SetActive(true);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
    }

    public void ShowLevelSelectPanel()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(true);
    }

    // Başlat / Devam Et Butonu
    public void PlayGame()
    {
        LoadLevel(lastUnlockedLevel);
    }

    // Seviye Yükleme
    public void LoadLevel(int levelIndex)
    {
        // Sahneler Build Settings'te Main Menu (0), Level 1 (1), Level 2 (2)... şeklinde sıralı olmalı
        SceneManager.LoadScene(levelIndex);
    }
}