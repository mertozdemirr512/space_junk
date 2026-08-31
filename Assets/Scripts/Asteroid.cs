using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private bool isSmall = false;
    public bool IsSmall => isSmall;
    private bool onGameScreen = false;
    private Collider2D asteroidCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        asteroidCollider = GetComponent<Collider2D>();
    }

     private void Start()
    {
        if (!isSmall)
        {
            float speed = 6f;
            float randomRotation = (Random.value > 0.5f) ? speed : -speed;
            rb.angularVelocity = randomRotation;
        }
        else
        {
            float speed = 4f;
            float randomRotation = (Random.value > 0.5f) ? speed : -speed;
            rb.angularVelocity = randomRotation;   
        }

    }

    private void Update()
    {
        if(!onGameScreen && IsVisibleToCamera())
        {
            onGameScreen = true;
        }

        if(onGameScreen && !IsVisibleToCamera())
        {
            AsteroidOutOfBounds();
        }
    }

    private bool IsVisibleToCamera()
    {
        if(Camera.main == null || asteroidCollider == null) return false;

        Bounds bounds = asteroidCollider.bounds;


        Vector3 minPoint = Camera.main.WorldToViewportPoint(bounds.min);
        Vector3 maxPoint = Camera.main.WorldToViewportPoint(bounds.max);

        bool isVisibleX = maxPoint.x >= 0f && minPoint.x <= 1f;
        bool isVisibleY = maxPoint.y >= 0f && minPoint.y <= 1f;

        if(isVisibleX && isVisibleY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void AsteroidOutOfBounds()
    {
        // Ekrandan çıktığı an rastgele bir kenarda yeniden doğdur
        RespawnFromRandomEdge();
    }

    private void RespawnFromRandomEdge()
    {
        if (Camera.main == null) return;

        // 0: Sol, 1: Sağ, 2: Alt, 3: Üst kenar
        int randomEdge = Random.Range(0, 4);
        Vector3 vpPos = Vector3.zero;

        switch (randomEdge)
        {
            case 0: // SOL KENAR (X=-0.03, Y=Rastgele)
                vpPos = new Vector3(-0.03f, Random.Range(0.1f, 0.9f), Camera.main.nearClipPlane);
                break;

            case 1: // SAĞ KENAR (X=1.03, Y=Rastgele)
                vpPos = new Vector3(1.03f, Random.Range(0.1f, 0.9f), Camera.main.nearClipPlane);
                break;

            case 2: // ALT KENAR (X=Rastgele, Y=-0.03)
                vpPos = new Vector3(Random.Range(0.1f, 0.9f), -0.03f, Camera.main.nearClipPlane);
                break;

            case 3: // ÜST KENAR (X=Rastgele, Y=1.03)
                vpPos = new Vector3(Random.Range(0.1f, 0.9f), 1.03f, Camera.main.nearClipPlane);
                break;
        }

        // Seçilen Viewport koordinatını dünya koordinatına çevir
        Vector3 newWorldPos = Camera.main.ViewportToWorldPoint(vpPos);
        newWorldPos.z = 0f;
        transform.position = newWorldPos;

        // Asteroidin ekranın içine doğru doğal bir süzülmeyle girmesini sağla
        if (rb != null)
        {
            Vector2 screenCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
            Vector2 pushDirection = (screenCenter - (Vector2)transform.position).normalized;

            // Rastgele bir giriş hızı ver
            float entrySpeed = isSmall ? Random.Range(2f, 4f) : Random.Range(1.5f, 3f);
            rb.velocity = pushDirection * entrySpeed;
        }

        // Tekrar ekrana tam girip onaylanana kadar tetiklenmeyi durdur
        onGameScreen = false;
    }

    /*public void SplitAsteroid()
    {
        if (isSmall) return; // Zaten küçükse tekrar bölünmesin

        if (smallAsteroidPrefab != null)
        {
            // Orijinal konumun biraz sağında ve solunda 2 küçük asteroid oluştur
            Vector3 pos = transform.position;
            Instantiate(smallAsteroidPrefab, pos + new Vector3(0.4f, 0.4f, 0), Quaternion.identity);
            Instantiate(smallAsteroidPrefab, pos + new Vector3(-0.4f, -0.4f, 0), Quaternion.identity);
        }

        Destroy(gameObject); // Büyük asteroidi yok et
    }*/

}
