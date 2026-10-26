using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private bool isSmall = false;
    public bool IsSmall => isSmall;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        float speed = isSmall ? 4f : 6f;
        float randomRotation = (Random.value > 0.5f) ? speed : -speed;
        rb.angularVelocity = randomRotation;
        if (!IsVisibleToCamera())
        {
            RespawnFromRandomEdge();
        }
    }

    private void Update()
    {

        if (!IsVisibleToCamera())
        {
            RespawnFromRandomEdge();
        }
    }

    private bool IsVisibleToCamera()
    {
        if (Camera.main == null) return false;
        Vector3 vpPos = Camera.main.WorldToViewportPoint(transform.position);
        bool isInsideX = vpPos.x >= -0.05f && vpPos.x <= 1f;
        bool isInsideY = vpPos.y >= -0.05f && vpPos.y <= 1f;

        return isInsideX && isInsideY;
    }

    private void RespawnFromRandomEdge()
    {
        if (Camera.main == null) return;

        int randomEdge = Random.Range(0, 4);
        Vector3 vpPos = Vector3.zero;

        switch (randomEdge)
        {
            case 0: 
                vpPos = new Vector3(-0.05f, Random.Range(0.15f, 0.85f), Camera.main.nearClipPlane);
                break;
            case 1:
                vpPos = new Vector3(1.05f, Random.Range(0.15f, 0.85f), Camera.main.nearClipPlane);
                break;
            case 2:
                vpPos = new Vector3(Random.Range(0.15f, 0.85f), -0.05f, Camera.main.nearClipPlane);
                break;
            case 3:
                vpPos = new Vector3(Random.Range(0.15f, 0.85f), 1.05f, Camera.main.nearClipPlane);
                break;
        }

        Vector3 newWorldPos = Camera.main.ViewportToWorldPoint(vpPos);
        newWorldPos.z = 0f;
        transform.position = newWorldPos;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            Vector2 screenCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
            Vector2 pushDirection = (screenCenter - (Vector2)transform.position).normalized;

            float entrySpeed = isSmall ? Random.Range(2f, 3.5f) : Random.Range(1.5f, 2.5f);
            rb.velocity = pushDirection * entrySpeed;
             rb.angularVelocity = (Random.value > 0.5f) ? 50f : -50f;
        }

    }
    public void EnterPyramidGate(Vector3 gatePosition)
    {
        if (rb != null)
        {   
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            rb.simulated = false; 

        StartCoroutine(ShrinkAndDestroyRoutine(gatePosition));
    }

    private IEnumerator ShrinkAndDestroyRoutine(Vector3 gatePos)
    {
        Vector3 initialScale = transform.localScale;
        Vector3 initialPos = transform.position;
        
        float duration = 1.5f; 
        float elapsed = 0f;
        Vector3 targetPos = new Vector3(gatePos.x, gatePos.y, initialPos.z);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;
            float smoothPercent = Mathf.SmoothStep(0f, 1f, percent);
            transform.position = Vector3.Lerp(initialPos, targetPos, smoothPercent);
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, smoothPercent);

            yield return null;
        }
        if (CompareTag("Asteroid_Small") || isSmall)
        {
            Debug.Log("A small asteroid entered the gate!");
            GoalGate.score += 0.5f;
        }
        else
        {
            Debug.Log("An asteroid entered the gate!");
            GoalGate.score += 1f;
        }

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.CheckScoreGoal();
        }

        Destroy(gameObject);
    }
}