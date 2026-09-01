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
    public static float score = 0;

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


                OnAsteroidScored(other.gameObject);
            }
        }
    }

    private void OnAsteroidScored(GameObject asteroidObj)
    {
        if(asteroidObj.CompareTag("Asteroid_Small"))
        {
            Debug.Log("A small asteroid made it");
            score+=0.5f;
            Destroy(asteroidObj);
        }
        else
        {
            Debug.Log("An asteroid made it");
            score++;
            Destroy(asteroidObj);   
        }


        if(GameManager.Instance != null)
        {
            GameManager.Instance.CheckScoreGoal();
        }
    }
}
