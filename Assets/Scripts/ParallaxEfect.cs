using UnityEngine;

public class ParallaxEfect : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; 
    
    private Vector3 lastPlayerPosition = Vector3.zero;
    private Vector3 playerDelta = Vector3.zero;
    [SerializeField] private float parallaxMultiplier = 0.02f;
    private Vector3 parallaxItem = Vector3.zero;

    [SerializeField] private float teleportThresholdX = 4f;
    [SerializeField] private float teleportThresholdY = 2f;

    private void Start()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }

        if (playerTransform != null)
        {
            lastPlayerPosition = playerTransform.position;
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        playerDelta.x = playerTransform.position.x - lastPlayerPosition.x;
        playerDelta.y = playerTransform.position.y - lastPlayerPosition.y;

        if (Mathf.Abs(playerDelta.x) < teleportThresholdX)
        {
            parallaxItem.x = -playerDelta.x * parallaxMultiplier;      
        }
        else
        {
            parallaxItem.x = 0f;
        }

        if (Mathf.Abs(playerDelta.y) < teleportThresholdY)
        {
            parallaxItem.y = -playerDelta.y * parallaxMultiplier;
        }
        else
        {
            parallaxItem.y = 0f;
        }

        transform.position += parallaxItem;

        lastPlayerPosition = playerTransform.position;
    }
}