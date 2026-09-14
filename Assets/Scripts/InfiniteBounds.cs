using System.Runtime.CompilerServices;
using UnityEngine;

public class InfiniteBounds : MonoBehaviour
{
    Rigidbody2D rb;
    Camera cam;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }
        private void FixedUpdate()
    {
        float sizeBuffer = transform.localScale.x/2f + 0.2f;
        Vector3 topRightCorner = cam.ViewportToWorldPoint(new Vector3(1,1,0));
        Vector3 bottomLeftCorner = cam.ViewportToWorldPoint(new Vector3(0,0,0));
         //1,1,0 is the top most right corner and the 0,0,0 is the down most left corner
    
        bool moveObject = false;
        Vector3 newPosition = rb.position;

        if(newPosition.x > topRightCorner.x + sizeBuffer)
        {
            newPosition.x = bottomLeftCorner.x - sizeBuffer;
            moveObject = true;
        }
        else if(newPosition.x < bottomLeftCorner.x - sizeBuffer)
        {
            newPosition.x = topRightCorner.x + sizeBuffer;
            moveObject = true;
        }
        if(newPosition.y > topRightCorner.y + sizeBuffer)
        {
            newPosition.y = bottomLeftCorner.y - sizeBuffer;
            moveObject = true;
        }
        else if(newPosition.y < bottomLeftCorner.y - sizeBuffer)
        {
            newPosition.y = topRightCorner.y + sizeBuffer;
            moveObject = true;
        }
        if (moveObject)
        { 
            rb.position = newPosition;
        }
       
    }

}

