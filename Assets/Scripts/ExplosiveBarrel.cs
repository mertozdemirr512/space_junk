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
            Explode(collision.gameObject);
            
        }
    }

    private void Explode(GameObject bigAsteroid)
    {
        // Patlamanın gerçekleştiği noktayı (varilin mevcut konumunu) referans olarak alıyoruz.
        Vector3 explosionPosition = transform.position;

        // 1. KÜÇÜK ASTEROİTLERİN DOĞURULMASI (SPAWN)
        // Atanmış bir küçük asteroid prefab'ı olup olmadığını kontrol ederek NullReference hatasını önlüyoruz.
        if (smallAsteroidPrefab != null)
        {
            // Doğacak 2 küçük asteroid için zıt yönlerde (Çapraz: Sağ-Üst ve Sol-Alt) offset pozisyonları hesaplıyoruz.
            // Bu işlem, yeni objelerin birbiriyle üst üste binip çakışmasını (collision glitch) engeller.
            Vector3 pos1 = explosionPosition + new Vector3(spawnOffset, spawnOffset, 0);
            Vector3 pos2 = explosionPosition + new Vector3(-spawnOffset, -spawnOffset, 0);

            // Küçük asteroid objelerini sahnede oluşturuyoruz.
            GameObject small1 = Instantiate(smallAsteroidPrefab, pos1, Quaternion.identity);
            GameObject small2 = Instantiate(smallAsteroidPrefab, pos2, Quaternion.identity);

            // Fizik motoru üzerinden fırlama hareketi verebilmek için her iki objenin Rigidbody2D bileşenlerini alıyoruz.
            Rigidbody2D rb1 = small1.GetComponent<Rigidbody2D>();
            Rigidbody2D rb2 = small2.GetComponent<Rigidbody2D>();

            // Normalize edilmiş yön vektörleri ile objelere patlama anında anlık (Impulse) itme kuvveti uyguluyoruz.
            if (rb1 != null) rb1.AddForce(new Vector2(1, 1).normalized * explosionForce, ForceMode2D.Impulse);
            if (rb2 != null) rb2.AddForce(new Vector2(-1, -1).normalized * explosionForce, ForceMode2D.Impulse);
        }

        // 2. PATLAMA GÖRSEL EFEKTİ (VFX)
        // Eğer görsel efekt prefab'ı atanmışsa patlama konumunda bir örneğini oluşturuyoruz.
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, explosionPosition, Quaternion.identity);
        }

        // Konsola bilgilendirme mesajı basarak patlama mantığının çalıştığını doğruluyoruz.
        Debug.Log("Varil patladı! Büyük asteroit bölündü.");

        // 3. BELLEK VE NESNE TEMİZLİĞİ (CLEANUP)
        // Çarpan büyük asteroidi ve patlayan varil objesini sahneden kaldırarak bellek sızıntısını önlüyoruz.
        Destroy(bigAsteroid);
        Destroy(gameObject);
}
}