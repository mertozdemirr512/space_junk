using UnityEngine;

public class PyramidMovement : MonoBehaviour
{
    public enum MovementType { None, RotateOnly, PatrolOnly, RotateAndPatrol }

    [Header("Movement Mode")]
    [SerializeField] private MovementType movementType = MovementType.RotateOnly;

    [Header("Rotation Settings")]
    [Tooltip("Saniyedeki dönme derecesi. Pozitif saat yönü tersi, negatif saat yönüdür.")]
    [SerializeField] private float rotationSpeed = 15f; 

    [Header("Patrol Settings")]
    [Tooltip("Piramidin gidip geleceği hedef offset noktası (Lokal pozisyona göre).")]
    [SerializeField] private Vector3 patrolOffset = new Vector3(3f, 0f, 0f);
    [SerializeField] private float patrolSpeed = 2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingToTarget = true;

    private void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + patrolOffset;
    }

    private void Update()
    {
        HandleRotation();
        HandlePatrol();
    }

    private void HandleRotation()
    {
        if (movementType == MovementType.RotateOnly || movementType == MovementType.RotateAndPatrol)
        {
            // Z ekseninde pürüzsüz kendi etrafında dönme
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }

    private void HandlePatrol()
    {
        if (movementType == MovementType.PatrolOnly || movementType == MovementType.RotateAndPatrol)
        {
            Vector3 destination = movingToTarget ? targetPosition : startPosition;
            
            // Hedefe doğru pürüzsüz kayma (Ping-pong Hareketi)
            transform.position = Vector3.MoveTowards(transform.position, destination, patrolSpeed * Time.deltaTime);

            // Hedefe varınca yön değiştir
            if (Vector3.Distance(transform.position, destination) < 0.01f)
            {
                movingToTarget = !movingToTarget;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Patika hattını Scene ekranında görselleştirme
        Gizmos.color = Color.cyan;
        Vector3 basePos = Application.isPlaying ? startPosition : transform.position;
        Gizmos.DrawLine(basePos, basePos + patrolOffset);
        Gizmos.DrawWireSphere(basePos + patrolOffset, 0.3f);
    }
}