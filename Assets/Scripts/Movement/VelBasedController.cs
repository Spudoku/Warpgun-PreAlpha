using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
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

    private float overallMaxSpeed;                      // the overall highest speed allowed for the rb


    [SerializeField] float jumpStrength = 10f;

    float curSpeed = 0f;                                // magnitude of current movement
    Vector3 moveDir;
    //[SerializeField] private float groundCheckDist = 0.1f;
    public LayerMask terrainLayer;
    [SerializeField] private bool grounded;

    // rigidbody damping!
    [SerializeField] private float moveDamping = 0f; // damping that occurs while taking move input
    [SerializeField] private float groundDamping = 0.9f; // drag that occurs while not taking move input and grounded
    [SerializeField] private float airDamping = 0.8f; // drag that occurs while not taking move input and airborne

    private float curDamping;

    Vector3 vertMove;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDir = new();
        grounded = false;
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();

        overallMaxSpeed = Mathf.Max(maxSpeed, airSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        vertMove = new(0, rb.linearVelocity.y, 0);
        // adjust damping
        if (grounded)
        {
            curDamping = groundDamping;
            if (Input.GetButtonDown("Jump"))
            {
                vertMove.y += jumpStrength;
            }
        }
        else
        {
            curDamping = airDamping;
            vertMove.y += Physics.gravity.y * Time.deltaTime;
        }


        moveDir = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
        if (Input.GetAxis("Vertical") > 0f || Input.GetAxis("Horizontal") > 0f)
        {
            curDamping = moveDamping;
            curSpeed += acceleration * Time.deltaTime;
        }

        // limit speed
        if (curSpeed > overallMaxSpeed)
        {
            curSpeed = overallMaxSpeed;
        }

        // manual control of rigidbody velocity
        rb.linearVelocity = Vector3.Lerp((moveDir * curSpeed) + vertMove, Vector3.zero, curDamping * Time.deltaTime);


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

    // Debugging GUI
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), $"Velocity: {rb.linearVelocity}");
    }

}

