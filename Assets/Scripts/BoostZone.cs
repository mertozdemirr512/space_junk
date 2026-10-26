using UnityEngine;

public class BoostZone : MonoBehaviour
{
    [Header("Boost Settings")]
    [SerializeField] private float boostForce = 15f; // Uygulanacak kuvvet miktarı

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Yalnızca Truck, Asteroid ve Asteroid_Small tag'ine sahip objeleri hızlandır
        if (other.CompareTag("Player") || other.CompareTag("Asteroid") || other.CompareTag("Asteroid_Small"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Objeyi Boost alanının ok yönünde (Yukarı / Transform.Up) ittir
                Vector2 boostDirection = transform.up;

                // Mevcut hıza anlık kuvvet (Impulse) ekle
                rb.AddForce(boostDirection * boostForce, ForceMode2D.Impulse);

                Debug.Log($"[BOOST] {other.gameObject.name} objesi {boostForce} kuvvetiyle hızlandırıldı!");
            }
        }
    }
}