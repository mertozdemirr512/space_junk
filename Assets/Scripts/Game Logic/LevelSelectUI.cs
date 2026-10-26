using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MainMenuManager mainMenuManager;
    [SerializeField] private GameObject levelButtonPrefab; // Izgara içine doğacak buton prefab'ı
    [SerializeField] private Transform buttonGridParent;   // Grid Layout Group olan Obje

    [Header("Level Settings")]
    [SerializeField] private int totalLevels = 10; // Toplam seviye sayısı

    private void Start()
    {
        GenerateLevelButtons();
    }

    private void GenerateLevelButtons()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        // Eski butonları temizle
        foreach (Transform child in buttonGridParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 1; i <= totalLevels; i++)
        {
            int levelIndex = i;
            GameObject newBtn = Instantiate(levelButtonPrefab, buttonGridParent);

            TMP_Text btnText = newBtn.GetComponentInChildren<TMP_Text>();
            if (btnText != null) btnText.text = levelIndex.ToString();

            Button btn = newBtn.GetComponent<Button>();

            if (levelIndex <= unlockedLevel)
            {
                btn.interactable = true;
                btn.onClick.AddListener(() => mainMenuManager.LoadLevel(levelIndex));
            }
            else
            {
                btn.interactable = false; // Kilitli seviye
            }
        }
    }
}