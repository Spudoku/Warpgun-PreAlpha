using System;
using UnityEngine;
using UnityEngine.XR.WSA;
[RequireComponent(typeof(Rigidbody))]
// "VelBased" means that movement is controlled by velocity.
// Requires a RigidBody.
// assumes the controller is a capsule
public class VelBasedController : MonoBehaviour
{
    [SerializeField] Collider groundChecker;            // must be a trigger!
    Rigidbody rb;
    [SerializeField] float maxSpeed = 10f;               // normalize movement to this speed!
    [SerializeField] float airSpeed = 2f;
    [SerializeField] float acceleration = 8f;           // acceleration in units/second/second

    [SerializeField] float deecelleration = 8f;         // negative acceleration in units/second/second
    [SerializeField] float airDecelleration = 8f;

    [SerializeField] float jumpStrength = 10f;

    float curSpeed = 0f;                                // magnitude of current movement
    Vector3 moveDir;
    [SerializeField] private float groundCheckDist = 0.1f;
    public LayerMask terrainLayer;
    [SerializeField] private bool grounded;

    private float halfHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDir = new();
        grounded = false;
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        halfHeight = capsuleCollider.height / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        // calculate speed;
        // take forward/backward input first
        // begin building moveDir vector

        Vector3 vertMove = new(0, rb.linearVelocity.y, 0);                              // take gravity into account for later
        //Debug.Log("Vertical velocity: " + vertMove);
        //grounded = Physics.Raycast(transform.position + new Vector3(0, -halfHeight, 0), Vector3.down, groundCheckDist, terrainLayer);
        Debug.DrawRay(transform.position + new Vector3(0, -halfHeight, 0), Vector3.down, Color.red, terrainLayer);
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {

            //moveDir = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
            moveDir += transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
            moveDir = moveDir.normalized;
            Debug.Log("MoveDir: " + moveDir);
            curSpeed += acceleration * Time.deltaTime;
            // decellerate
            if (grounded)
            {
                // limiting ground speed
                if (curSpeed > maxSpeed)
                {
                    curSpeed = Mathf.Max(maxSpeed, curSpeed -= deecelleration * Time.deltaTime);
                }
            }
            else
            {
                // limiting airspeed
                if (curSpeed > airSpeed)
                {
                    curSpeed = Mathf.Max(airSpeed, curSpeed -= airDecelleration * Time.deltaTime);
                }
            }
        }
        else
        {
            curSpeed -= deecelleration * Time.deltaTime;
        }

        // speed cannot go below 0 (SPEED, not VELOCITY)
        if (curSpeed < 0f) curSpeed = 0f;

        if (grounded && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Attempting jump!");
            vertMove.y += jumpStrength;
        }






        // set linear velocity to moveDir * curSpeed;
        rb.linearVelocity = vertMove + (moveDir * curSpeed);

        // lock angular velocity to 0;
        rb.angularVelocity = new();

        //Debug.Log("rb linearvelocity: " + rb.linearVelocity + ", vertMove" + vertMove + ", moveDir: " + moveDir + ", curSpeed: " + curSpeed);

    }

    void OnTriggerStay(Collider other)
    {
        int targetLayer = LayerMask.NameToLayer("Terrain"); // Replace with your layer name
        Debug.Log("Other layer: " + other.gameObject.layer + ", targetlayer: " + targetLayer);
        if (other.gameObject.layer == targetLayer)
        {
            grounded = true;

        }
    }

    void OnTriggerExit(Collider other)
    {
        int targetLayer = LayerMask.NameToLayer("Terrain"); // Replace with your layer name
        if (other.gameObject.layer == targetLayer)
        {
            grounded = false;

        }
    }
}

