using UnityEngine;

public class OneWayBarrier : MonoBehaviour
{
    [SerializeField] private bool allowAsteroidPass = true;

    private void Start()
    {
        if (!allowAsteroidPass) return;

        Collider2D barrierCollider = GetComponent<Collider2D>();

        // Sahnedeki tüm büyük ve küçük asteroidleri bulup çarpışmayı önceden iptal eder
        foreach (var asteroid in GameObject.FindGameObjectsWithTag("Asteroid"))
            Physics2D.IgnoreCollision(asteroid.GetComponent<Collider2D>(), barrierCollider, true);

        foreach (var smallAsteroid in GameObject.FindGameObjectsWithTag("Asteroid_Small"))
            Physics2D.IgnoreCollision(smallAsteroid.GetComponent<Collider2D>(), barrierCollider, true);
    }
}