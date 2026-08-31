using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalGate : MonoBehaviour
{
    public enum GateState{Open, Closed, SmallOnly};

    [SerializeField] GateState currentState = GateState.Open;
    public Sprite openSprite;
    public Sprite closedSprite;
    public Sprite smallOnlySprite;

    private SpriteRenderer spriteRenderer;
    private Collider2D gateCollider;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gateCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        UpdateGateState();
    }

    // Inspector'dan durum değiştirildiğinde görseli ve collider'ı günceller
    public void SetGateState(GateState newState)
    {
        currentState = newState;
        UpdateGateState();
    }

    private void UpdateGateState()
    {   
        if (spriteRenderer == null) return;

        switch (currentState)
        {
            case GateState.Open:
                spriteRenderer.enabled = (openSprite != null);
                spriteRenderer.sprite = openSprite;
                if (gateCollider != null) gateCollider.enabled = true;
                break;

            case GateState.Closed:
                spriteRenderer.enabled = (closedSprite != null);
                spriteRenderer.sprite = closedSprite;
                if (gateCollider != null) gateCollider.enabled = false;
                break;

            case GateState.SmallOnly:
                spriteRenderer.enabled = (smallOnlySprite != null);
                spriteRenderer.sprite = smallOnlySprite;
                if (gateCollider != null) gateCollider.enabled = true;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (currentState == GateState.Closed) return;

        if (other.CompareTag("Asteroid"))
        {
            Asteroid asteroid = other.GetComponent<Asteroid>();

            if (asteroid != null)
            {
                // Eğer kapı sadece küçükleri alıyorsa ve giren asteroid büyükse reddet
                if (currentState == GateState.SmallOnly && !asteroid.IsSmall)
                {
                    Debug.Log("Bu kapı sadece küçük asteroidleri kabul eder!");
                    return;
                }

                // Asteroid başarılı şekilde sokuldu
                OnAsteroidScored(other.gameObject);
            }
        }
    }

    private void OnAsteroidScored(GameObject asteroidObj)
    {
        Debug.Log("Asteroid piramide girdi!");
        Destroy(asteroidObj);

        // İleride buraya: LevelManager.Instance.OnAsteroidScored();
    }
}
