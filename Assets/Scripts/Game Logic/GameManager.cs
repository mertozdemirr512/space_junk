using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Win,
    Loss,
    Gameplay,
    Menu,
    Pause
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("UI Panels (Opsiyonel / Otomatik Bulunur)")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject lossPanel;
    [SerializeField] private GameObject pausePanel;

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
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GoalGate.score = 0;

        // Otomatik Panel Bağlama: Inspector boşsa sahneden LevelCore/Canvas içindekileri bulur
        FindPanelsAutomatically();

        ChangeGameState(GameState.Gameplay);
    }

    /// <summary>
    /// Sahnede veya LevelCore prefab'ı içindeki panelleri ismen otomatik arayıp bağlar.
    /// </summary>
    private void FindPanelsAutomatically()
    {
        if (pausePanel == null)
        {
            // Sahnede pasif bile olsa "PausePanel" isimli objeyi bulur
            pausePanel = FindObjectInSceneIncludingInactive("PausePanel");
        }

        if (winPanel == null)
        {
            winPanel = FindObjectInSceneIncludingInactive("WinPanel");
        }

        if (lossPanel == null)
        {
            lossPanel = FindObjectInSceneIncludingInactive("LossPanel");
        }

        // Başlangıçta paneller kapalı olsun
        if (winPanel) winPanel.SetActive(false);
        if (lossPanel) lossPanel.SetActive(false);
        if (pausePanel) pausePanel.SetActive(false);
    }

    private GameObject FindObjectInSceneIncludingInactive(string objectName)
    {
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in allTransforms)
        {
            // Yalnızca aktif sahnedeki ve adı eşleşen objeleri seç (Asset/Prefab klasöründekileri hariç tut)
            if (t.gameObject.name == objectName && t.gameObject.scene.isLoaded)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (currentState == GameState.Gameplay)
        {
            ChangeGameState(GameState.Pause);
        }
        else if (currentState == GameState.Pause)
        {
            ChangeGameState(GameState.Gameplay);
        }
    }
    
    public void ChangeGameState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.Win:
                Time.timeScale = 0f;
                if (winPanel != null) winPanel.SetActive(true);
                if (pausePanel != null) pausePanel.SetActive(false);
                if (AudioManager.Instance != null) AudioManager.Instance.StopBGM();
                break;

            case GameState.Loss:
                Time.timeScale = 0f;
                if (lossPanel != null) lossPanel.SetActive(true);
                if (pausePanel != null) pausePanel.SetActive(false);
                if (AudioManager.Instance != null) AudioManager.Instance.StopBGM();
                break;

            case GameState.Pause:
                Time.timeScale = 0f;
                if (pausePanel != null) pausePanel.SetActive(true);
                if (AudioManager.Instance != null) AudioManager.Instance.PauseBGM();
                break;

            case GameState.Gameplay:
                Time.timeScale = 1f;
                if (pausePanel != null) pausePanel.SetActive(false);
                if (AudioManager.Instance != null) 
                {
                    AudioManager.Instance.PlayBGM(AudioManager.Instance.gameplayBGM);
                    AudioManager.Instance.ResumeBGM();
                }
                break;

            case GameState.Menu:
                Time.timeScale = 1f;
                if (AudioManager.Instance != null) 
                AudioManager.Instance.PlayBGM(AudioManager.Instance.mainMenuBGM);
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