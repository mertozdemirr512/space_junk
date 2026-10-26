using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level Data Configuration")]
    [SerializeField] private LevelData currentLevelData;

    [Header("Scene References (Opsiyonel)")]
    [SerializeField] private GameObject pyramidInScene;

    public LevelData CurrentLevelData => currentLevelData;

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
        if (currentLevelData != null)
        {
            SetupSceneLevel();
        }
        else
        {
            Debug.LogError("[LevelManager] Current Level Data atanmamış!");
        }
    }

    private void SetupSceneLevel()
    {
        GoalGate.score = 0;

        if (pyramidInScene == null)
        {
            pyramidInScene = GameObject.FindWithTag("Pyramid");
        }

        if (pyramidInScene != null)
        {
            GoalGate[] gates = pyramidInScene.GetComponentsInChildren<GoalGate>();
            for (int i = 0; i < gates.Length && i < currentLevelData.gateStates.Length; i++)
            {
                gates[i].SetGateState(currentLevelData.gateStates[i]);
            }
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeGameState(GameState.Gameplay);
        }
    }

    public void CheckScoreGoal()
    {
        if (currentLevelData == null || GameManager.Instance == null) return;

        if (GoalGate.score >= currentLevelData.requiredScoreGoal)
        {
            UnlockNextLevel();
            GameManager.Instance.OnLevelWin();
        }
    }

    public void ResumeGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeGameState(GameState.Gameplay);
        }
    }

    private void UnlockNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int highestUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (currentSceneIndex >= highestUnlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentSceneIndex + 1);
            PlayerPrefs.Save();
        }
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("[LEVEL SYSTEM] Tüm seviyeler tamamlandı! Ana menüye dönülüyor.");
            GoToMainMenu();
        }
    }

    public void RestartCurrentLevel()
    {
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}