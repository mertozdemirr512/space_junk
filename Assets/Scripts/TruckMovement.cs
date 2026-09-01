using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;

public class TruckMovement: MonoBehaviour
{
   [SerializeField] private float moveSpeed = 3f;
   [SerializeField] private float turnSpeed = 0.3f;
   [SerializeField] private float reverseSpeedMultiplier = 0.3f;
   [SerializeField] private Vector2 centerOfMassOffset = new Vector2(0f, -0.8f);
   public GameObject exhaustFlame;
   public GameObject magnetEffect;
   private float moveInput;
   private float turnInput;
   

   private Rigidbody2D rb;

   private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        rb.centerOfMass = centerOfMassOffset;
        if (exhaustFlame) exhaustFlame.SetActive(false);
        if (magnetEffect) magnetEffect.SetActive(false);
    }

    private void FixedUpdate()
    {
        float currentSpeed = moveSpeed;

        if(moveInput < 0)
        {
            currentSpeed *= reverseSpeedMultiplier;
        }

        rb.AddForce(transform.up * moveInput * currentSpeed);
        rb.AddTorque(-turnInput * turnSpeed);

    }

       private void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        if(exhaustFlame != null)
        {
            bool isMovingForward = moveInput > 0.05f; 
            exhaustFlame.SetActive(isMovingForward);
        
        }
    }

}

    