using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject smallAsteroidPrefab; // İki adet üretilecek küçük asteroit prefab'ı
    [SerializeField] private float spawnOffset = 0.5f; // Küçük asteroitlerin doğacağı mesafe
    [SerializeField] private float explosionForce = 3f; // Doğarken dışarı fırlama hızı

    [Header("Effects")]
    [SerializeField] private GameObject explosionEffectPrefab; // Varsa patlama görsel/partikül efekti

    [Header("Camera Shake Settings")]
    [SerializeField] private float shakeDuration = 0.25f; // Ekran sallanma süresi
    [SerializeField] private float shakeMagnitude = 0.35f; // Ekran sallanma şiddeti

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Çarpan obje Asteroid mi kontrol et
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();

            // Çarpan asteroit var ise patlamayı tetikle
            Explode(collision.gameObject);
        }
    }

    private void Explode(GameObject bigAsteroid)
    {
        // Kamyon/Launcher tutulan asteroiti patlatıyorsa launcher'ı sıfırla
        AsteroidLauncher launcher = FindObjectOfType<AsteroidLauncher>();
        if (launcher != null)
        {
            launcher.ResetLauncher();
        }

        // Patlamanın gerçekleştiği noktayı (varilin mevcut konumunu) referans olarak alıyoruz.
        Vector3 explosionPosition = transform.position;

        // 1. KAMERA SALLANMA EFEKTİ (CAMERA SHAKE)
        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake(shakeDuration, shakeMagnitude);
        }

        // 2. KÜÇÜK ASTEROİTLERİN DOĞURULMASI (SPAWN)
        if (smallAsteroidPrefab != null)
        {
            Vector3 pos1 = explosionPosition + new Vector3(spawnOffset, spawnOffset, 0);
            Vector3 pos2 = explosionPosition + new Vector3(-spawnOffset, -spawnOffset, 0);

            GameObject small1 = Instantiate(smallAsteroidPrefab, pos1, Quaternion.identity);
            GameObject small2 = Instantiate(smallAsteroidPrefab, pos2, Quaternion.identity);

            Rigidbody2D rb1 = small1.GetComponent<Rigidbody2D>();
            Rigidbody2D rb2 = small2.GetComponent<Rigidbody2D>();

            if (rb1 != null) rb1.AddForce(new Vector2(1, 1).normalized * explosionForce, ForceMode2D.Impulse);
            if (rb2 != null) rb2.AddForce(new Vector2(-1, -1).normalized * explosionForce, ForceMode2D.Impulse);
        }

        // 3. PATLAMA GÖRSEL EFEKTİ (VFX)
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, explosionPosition, Quaternion.identity);
        }

        Debug.Log("Varil patladı! Büyük asteroit bölündü ve launcher resetlendi.");

        // 4. BELLEK VE NESNE TEMİZLİĞİ (CLEANUP)
        Destroy(bigAsteroid);
        Destroy(gameObject);
    }
}