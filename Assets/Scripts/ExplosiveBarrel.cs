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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Çarpan obje Asteroid mi kontrol et
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();

            // Çarpan asteroit var ise ve BÜYÜK ise patlamayı tetikle
            if (asteroid != null && !asteroid.IsSmall)
            {
                Explode(collision.gameObject);
            }
        }
    }

    private void Explode(GameObject bigAsteroid)
    {
        Vector3 explosionPosition = transform.position;

        // 1. İki adet küçük asteroit oluştur
        if (smallAsteroidPrefab != null)
        {
            // Sağ-Üst ve Sol-Alt yönlerinde 2 küçük asteroit doğur
            Vector3 pos1 = explosionPosition + new Vector3(spawnOffset, spawnOffset, 0);
            Vector3 pos2 = explosionPosition + new Vector3(-spawnOffset, -spawnOffset, 0);

            GameObject small1 = Instantiate(smallAsteroidPrefab, pos1, Quaternion.identity);
            GameObject small2 = Instantiate(smallAsteroidPrefab, pos2, Quaternion.identity);

            // Küçük asteroitlere zıt yönlerde minik bir fırlama kuvveti ver
            Rigidbody2D rb1 = small1.GetComponent<Rigidbody2D>();
            Rigidbody2D rb2 = small2.GetComponent<Rigidbody2D>();

            if (rb1 != null) rb1.AddForce(new Vector2(1, 1).normalized * explosionForce, ForceMode2D.Impulse);
            if (rb2 != null) rb2.AddForce(new Vector2(-1, -1).normalized * explosionForce, ForceMode2D.Impulse);
        }

        // 2. Patlama Efekti (Opsiyonel)
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, explosionPosition, Quaternion.identity);
        }

        Debug.Log("Varil patladı! Büyük asteroit bölündü.");

        // 3. Büyük asteroiti ve varilin kendisini yok et
        Destroy(bigAsteroid);
        Destroy(gameObject);
    }
}