using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AsteroidLauncher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform magnetPoint;
    [SerializeField] private Transform arrowTransform;
    [SerializeField] private LayerMask asteroidLayer;
    [SerializeField] private Rigidbody2D truckRb;

    [Header("Cooldown Radial UI")]
    [Tooltip("Dairesel Cooldown görseli (Image Type: Filled, Fill Method: Radial 360)")]
    [SerializeField] private Image cooldownRadialFill;
    [Tooltip("Cooldown süresini gösteren yazı (TMP)")]
    [SerializeField] private TMP_Text cooldownText;
    [Tooltip("Cooldown UI'ını kapsayan Obje (Kapanıp açılması için)")]
    [SerializeField] private GameObject cooldownUIParent;

    [Header("Controls")]
    [SerializeField] private KeyCode actionKey = KeyCode.Space;

    [Header("Settings")]
    [SerializeField] private float catchRadius = 3f;
    [SerializeField] private float minLaunchForce = 10f;
    [SerializeField] private float maxLaunchForce = 35f;
    [SerializeField] private float maxChargeTime = 2f;
    [SerializeField] private float holdOffset = 0.8f;
    
    [Header("Auto Release / Timeout")]
    [SerializeField] private float maxHoldTime = 5f; // Belli süre atılmazsa otomatik sıfırlanır
    private float currentHoldTimer = 0f;

    [Header("Cooldown Settings")]
    [SerializeField] private float cooldownDuration = 8f;
    private float nextLaunchTime = 0f;

    private Rigidbody2D caughtAsteroid;
    private bool isLocked = false;
    private bool isCharging = false;
    private float currentChargeTime = 0f;

    private Vector3 originalArrowScale;

    private void Awake()
    {
        if (magnetPoint == null) magnetPoint = transform;
        if (truckRb == null) truckRb = GetComponent<Rigidbody2D>();

        if (arrowTransform != null)
        {
            originalArrowScale = arrowTransform.localScale;
            arrowTransform.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Cooldown UI Güncellemesi
        UpdateCooldownUI();

        // GÜVENLİK: Eğer kilitliysek ama tutulan asteroit yok edildiyse anında sıfırla
        if (isLocked && caughtAsteroid == null)
        {
            ResetLauncher();
            return;
        }

        // 1. YAKALAMA (Yalnızca Cooldown Dolduysa Çalışır)
        if (Input.GetKeyDown(actionKey) && !isLocked)
        {
            if (Time.time >= nextLaunchTime)
            {
                TryCatchAsteroid();
            }
            else
            {
                float remaining = nextLaunchTime - Time.time;
                Debug.Log($"Fırlatıcı henüz hazır değil! Kalan süre: {remaining:F1}s");
            }
        }

        // 2. KİLİTLİ DURUM VE ŞARJ
        if (isLocked && caughtAsteroid != null)
        {
            Vector2 holdPosition = (Vector2)magnetPoint.position + ((Vector2)transform.up * holdOffset);
            caughtAsteroid.position = holdPosition;

            // Tutma Zaman Aşımı Kontrolü (Overcharge / Auto Cancel)
            currentHoldTimer += Time.deltaTime;
            if (currentHoldTimer >= maxHoldTime)
            {
                Debug.Log("Asteroit çok uzun süre tutuldu! Şarj iptal edildi ve asteroit serbest bırakıldı.");
                ResetLauncher();
                return;
            }

            if (Input.GetKey(actionKey))
            {
                if (!isCharging)
                {
                    isCharging = true;
                    currentChargeTime = 0f;
                    if (arrowTransform != null) arrowTransform.gameObject.SetActive(true);
                }

                currentChargeTime += Time.deltaTime;
                currentChargeTime = Mathf.Clamp(currentChargeTime, 0f, maxChargeTime);

                UpdateArrowScale();
            }

            if (Input.GetKeyUp(actionKey) && isCharging)
            {
                LaunchAsteroid();
            }
        }
    }

    private void UpdateCooldownUI()
    {
        if (Time.time < nextLaunchTime)
        {
            if (cooldownUIParent != null && !cooldownUIParent.activeSelf)
                cooldownUIParent.SetActive(true);

            float remainingTime = nextLaunchTime - Time.time;
            float fillRatio = remainingTime / cooldownDuration;

            // Saat yönünde dairesel boşalma (Radial 360)
            if (cooldownRadialFill != null)
                cooldownRadialFill.fillAmount = fillRatio;

            if (cooldownText != null)
                cooldownText.text = remainingTime.ToString("F1") + "s";
        }
        else
        {
            // Bekleme süresi bitti - UI Gizle
            if (cooldownRadialFill != null) cooldownRadialFill.fillAmount = 0f;
            if (cooldownUIParent != null && cooldownUIParent.activeSelf)
                cooldownUIParent.SetActive(false);
        }
    }

    private void TryCatchAsteroid()
    {
        Collider2D hit = Physics2D.OverlapCircle(magnetPoint.position, catchRadius, asteroidLayer);

        if (hit != null)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                caughtAsteroid = rb;
                isLocked = true;
                currentHoldTimer = 0f;

                caughtAsteroid.isKinematic = true;
                caughtAsteroid.velocity = Vector2.zero;

                if (truckRb != null)
                {
                    truckRb.constraints = RigidbodyConstraints2D.FreezePositionX | 
                                          RigidbodyConstraints2D.FreezePositionY;
                }
            }
        }
    }

    private void UpdateArrowScale()
    {
        if (arrowTransform == null) return;

        float chargePercent = currentChargeTime / maxChargeTime;
        Vector3 newScale = originalArrowScale;
        newScale.y = Mathf.Lerp(originalArrowScale.y, originalArrowScale.y * 3f, chargePercent);
        arrowTransform.localScale = newScale;
    }

    private void LaunchAsteroid()
    {
        float chargePercent = currentChargeTime / maxChargeTime;
        float currentForce = Mathf.Lerp(minLaunchForce, maxLaunchForce, chargePercent);

        Vector2 launchDirection = transform.up;
        Vector2 launchPosition = (Vector2)magnetPoint.position + (launchDirection * holdOffset);
        caughtAsteroid.position = launchPosition;

        // Fırlatıldığı an Cooldown sayacını başlatıyoruz
        nextLaunchTime = Time.time + cooldownDuration;

        // Asteroide kuvvet uygula ve sıfırla
        if (caughtAsteroid != null)
        {
            caughtAsteroid.isKinematic = false;
            caughtAsteroid.AddForce(launchDirection * currentForce, ForceMode2D.Impulse);
        }

        ResetLauncher();
    }

    /// <summary>
    /// Fırlatıcıyı sıfırlayarak kamyonu kilitli durumdan çıkarır ve asteroiti serbest bırakır.
    /// </summary>
    public void ResetLauncher()
    {
        if (caughtAsteroid != null)
        {
            caughtAsteroid.isKinematic = false;
        }

        isLocked = false;
        isCharging = false;
        caughtAsteroid = null;
        currentChargeTime = 0f;
        currentHoldTimer = 0f;

        if (arrowTransform != null)
        {
            arrowTransform.localScale = originalArrowScale;
            arrowTransform.gameObject.SetActive(false);
        }

        if (truckRb != null)
        {
            truckRb.constraints = RigidbodyConstraints2D.None;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 origin = magnetPoint != null ? magnetPoint.position : transform.position;
        Gizmos.DrawWireSphere(origin, catchRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(origin + (transform.up * holdOffset), 0.2f);
    }
}