using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalGate : MonoBehaviour
{
    public enum GateState { Open, Closed, SmallOnly };
    public enum GateOrientation { Auto, Top, Bottom, Left, Right };

    [Header("Gate Settings")]
    [Tooltip("Kapının yönü. Eğer her kapının Z rotasyonu piramidin ortasına bakacak şekilde ayarlıysa Auto tutabilirsiniz.")]
    [SerializeField] private GateOrientation orientation = GateOrientation.Auto;

    [Tooltip("Kapının merkezinden piramidin boğazına (içeriye) doğru olan mesafe.")]
    [SerializeField] private float mouthOffset = 1.2f;

    [SerializeField] private GateState currentState = GateState.Open;
    public static float score = 0;

    private Animator animator;
    private Collider2D gateCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gateCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        UpdateGateState();
    }

    public void SetGateState(GateState newState)
    {
        currentState = newState;
        UpdateGateState();
    }

    private void UpdateGateState()
    {   
        if (animator == null) return;

        animator.SetInteger("GateState", (int)currentState);

        switch (currentState)
        {
            case GateState.Open:
            case GateState.SmallOnly:
                if (gateCollider != null) gateCollider.enabled = true;
                break;

            case GateState.Closed:
                if (gateCollider != null) gateCollider.enabled = false;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (currentState == GateState.Closed) return;

        if (other.CompareTag("Asteroid") || other.CompareTag("Asteroid_Small"))
        {
            Asteroid asteroid = other.GetComponent<Asteroid>();

            if (asteroid != null)
            {
                if (currentState == GateState.SmallOnly && !asteroid.IsSmall)
                {
                    Debug.Log("Bu kapı sadece küçük asteroidleri kabul eder!");
                    return;
                }

                asteroid.EnterPyramidGate(GetTargetMouthPosition());
            }
        }
    }

    /// <summary>
    /// Piramit ve Kapı döndükçe kırmızı noktanın lokal rotasyona göre dönmesini sağlar.
    /// </summary>
    public Vector3 GetTargetMouthPosition()
    {
        Vector2 localDirection = Vector2.zero;

        // Auto modunda kapının objesel lokal yönüne göre offset belirleme
        switch (orientation)
        {
            case GateOrientation.Top:
                localDirection = Vector2.down; // Üst kapı -> Lokal aşağı kayar
                break;
            case GateOrientation.Bottom:
                localDirection = Vector2.up; // Alt kapı -> Lokal yukarı kayar
                break;
            case GateOrientation.Left:
                localDirection = Vector2.right; // Sol kapı -> Lokal sağa kayar
                break;
            case GateOrientation.Right:
                localDirection = Vector2.left; // Sağ kapı -> Lokal sola kayar
                break;
            case GateOrientation.Auto:
                // Kapının kendi Z rotasyonu sağa/sola/yukarıya bakıyorsa
                // -transform.up doğrudan piramidin merkezine bakar
                return transform.position - (transform.up * mouthOffset);
        }

        // Lokal yönü dönmüş olan Dünya Yönüne (World Space) çeviriyoruz
        Vector3 worldOffset = transform.TransformDirection(localDirection * mouthOffset);
        return transform.position + worldOffset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetTargetMouthPosition(), 0.07f);
    }
}