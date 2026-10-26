using UnityEngine;

public class OneWayBarrier : MonoBehaviour
{
    [SerializeField] private bool allowAsteroidPass = true;
    private Collider2D barrierCollider;

    private void Awake()
    {
        barrierCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        IgnoreExistingAsteroids();
    }

    private void Update()
    {
        if (!allowAsteroidPass || barrierCollider == null) return;

        // Bariyerin bulunduğu alandaki veya hemen yakınındaki tüm collider'ları anlık tara
        Bounds bounds = barrierCollider.bounds;
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f);

        foreach (var col in hitColliders)
        {
            if (col != null && col != barrierCollider)
            {
                if (col.CompareTag("Asteroid") || col.CompareTag("Asteroid_Small"))
                {
                    Physics2D.IgnoreCollision(col, barrierCollider, true);
                }
            }
        }
    }

    private void IgnoreExistingAsteroids()
    {
        if (!allowAsteroidPass || barrierCollider == null) return;

        foreach (var asteroid in GameObject.FindGameObjectsWithTag("Asteroid"))
        {
            Collider2D col = asteroid.GetComponent<Collider2D>();
            if (col != null) Physics2D.IgnoreCollision(col, barrierCollider, true);
        }

        foreach (var smallAsteroid in GameObject.FindGameObjectsWithTag("Asteroid_Small"))
        {
            Collider2D col = smallAsteroid.GetComponent<Collider2D>();
            if (col != null) Physics2D.IgnoreCollision(col, barrierCollider, true);
        }
    }
}