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
        GoalGate.score = 0; // Her level başında skoru sıfırla

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

    // GoalGate tarafından her skor yapıldığında çağrılacak metot
    public void CheckScoreGoal()
    {
        if (currentLevelData == null || GameManager.Instance == null) return;

        if (GoalGate.score >= currentLevelData.requiredScoreGoal)
        {
            GameManager.Instance.OnLevelWin();
        }
    }

    public void LoadNextLevel()
        {
            // Zaman акışını normale döndür (Win durumunda dondurulmuş olabilir)
            Time.timeScale = 1f;

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            Debug.Log($"[LEVEL SYSTEM] Şu anki Sahne Index: {currentSceneIndex} | Hedef Sahne Index: {nextSceneIndex} | Toplam Sahne Sayısı: {SceneManager.sceneCountInBuildSettings}");

            // Build Settings'te bir sonraki sahne var mı kontrol et
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("[LEVEL SYSTEM] Tüm seviyeler tamamlandı! Ana menüye dönülüyor.");
                SceneManager.LoadScene(0); // Sahne 0 (Ana Menü)
            }
        }

        // Mevcut seviyeyi yeniden başlatır (Retry / Restart Butonu İçin)
        public void RestartCurrentLevel()
        {
            Time.timeScale = 1f;
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    
    
}