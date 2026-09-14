using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [Header("Default Shake Settings")]
    [Tooltip("Varsayılan sallanma süresi (saniye)")]
    [SerializeField] private float defaultDuration = 0.25f;

    [Tooltip("Varsayılan sallanma şiddeti/genliği")]
    [SerializeField] private float defaultMagnitude = 0.35f;

    private Vector3 initialPosition;

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
        // Kameranın orijinal başlangıç pozisyonunu kaydediyoruz
        initialPosition = transform.localPosition;
    }

    /// <summary>
    /// Inspector'dan girilen varsayılan değerlerle sallatır.
    /// </summary>
    public void Shake()
    {
        Shake(defaultDuration, defaultMagnitude);
    }

    /// <summary>
    /// Özel süre ve şiddet değerleri verilerek sallatır.
    /// </summary>
    public void Shake(float duration, float magnitude)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Orijinal pozisyon üzerinden rastgele offset veriyoruz
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(initialPosition.x + x, initialPosition.y + y, initialPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Sallanma bitince kamerayı tam sıfır pozisyonuna getir
        transform.localPosition = initialPosition;
    }
}