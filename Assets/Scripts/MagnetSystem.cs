using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetSystem : MonoBehaviour
{
    public Transform magnetPoint;
   [SerializeField] private float attractionForce = 20f;
   [SerializeField] private float attractionRadius = 3f;
   [Range(10f, 180f)]
   [SerializeField] private float viewAngle = 60f;

   public LayerMask asteroidLayer;
   public KeyCode magnetKey = KeyCode.Space;

   [SerializeField] public float maxEnergy = 5f;
   private float currentEnergy;
   public GameObject Magnet_Effect;
   private Animator effectAnimator;

   public System.Action<float, float> OnEnergyChanged; // (currentEnergy, maxEnergy)
   private bool isMagnetActive = false;
   private void Awake()
   {
      if (magnetPoint == null) magnetPoint = transform;

      currentEnergy = maxEnergy;
      if(Magnet_Effect != null)
        {
            effectAnimator = Magnet_Effect.GetComponent<Animator>();
            Magnet_Effect.SetActive(false);
        }

    }

    private void Update()
    {
        if (Input.GetKey(magnetKey) && currentEnergy > 0)
                {   
                    if (!isMagnetActive) // Mıknatıs yeni açıldığında
                    {
                        isMagnetActive = true;
                        if (Magnet_Effect != null)
                        {
                            Magnet_Effect.SetActive(true);
                            if (effectAnimator != null) effectAnimator.Play(0); // Animasyonu baştan oynat
                        }
                    }
                    //ConsumeEnergy();
                }
                else
                {
                    if (isMagnetActive) // Mıknatıs kapandığında
                    {
                        isMagnetActive = false;
                        if (Magnet_Effect != null)
                        {
                            Magnet_Effect.SetActive(false);
                        }
                    }
                }
    }

    private void FixedUpdate()
    {
        if (isMagnetActive)
        {
            AttractAsteroids();
        }
    }

    private void AttractAsteroids()
    {
        Vector2 origin = magnetPoint.position;

        Vector2 facingDirection = transform.up; 

        Collider2D[] asteroids = Physics2D.OverlapCircleAll(origin, attractionRadius, asteroidLayer);

        foreach (Collider2D asteroid in asteroids)
        {
            Rigidbody2D asteroidRb = asteroid.GetComponent<Rigidbody2D>();
            if (asteroidRb != null)
            {

                Vector2 dirToAsteroid = (asteroidRb.position - origin).normalized;
                

                float angle = Vector2.Angle(facingDirection, dirToAsteroid);

                if (angle <= viewAngle / 2f)
                {

                    Vector2 pullDirection = (origin - asteroidRb.position).normalized;
                    asteroidRb.AddForce(pullDirection * attractionForce, ForceMode2D.Force);
                }
            }
        }
    }

   /* private void ConsumeEnergy()
    {
        currentEnergy -= Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
        
     
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }*/
    private void OnDrawGizmosSelected()
    {
        Vector3 origin = magnetPoint != null ? magnetPoint.position : transform.position;
        Vector3 facing = transform.up; 

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(origin, attractionRadius);

        Vector3 leftRayDirection = Quaternion.AngleAxis(-viewAngle / 2f, Vector3.forward) * facing;
        Vector3 rightRayDirection = Quaternion.AngleAxis(viewAngle / 2f, Vector3.forward) * facing;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin, leftRayDirection * attractionRadius);
        Gizmos.DrawRay(origin, rightRayDirection * attractionRadius);
    }
}