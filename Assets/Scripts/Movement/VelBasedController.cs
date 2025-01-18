using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
// "VelBased" means that movement is controlled by velocity.
// Requires a RigidBody.
public class VelBasedController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float maxSpeed = 10f;               // normalize movement to this speed!
    [SerializeField] float acceleration = 8f;           // acceleration in units/second/second

    [SerializeField] float deecelleration = 8f;         // negative acceleration in units/second/second

    float curSpeed = 0f;                                // magnitude of current movement
    Vector3 moveDir;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDir = new();
    }

    // Update is called once per frame
    void Update()
    {
        // calculate speed;
        // take forward/backward input first
        // begin building moveDir vector

        Vector3 vertMove = new(0, rb.linearVelocity.y, 0);                              // take gravity into account for later
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {

            moveDir = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
            curSpeed += acceleration * Time.deltaTime;
            // limit curSpeed
            if (curSpeed > maxSpeed) curSpeed = maxSpeed;
        }
        else
        {
            curSpeed -= deecelleration * Time.deltaTime;
        }
        //Debug.Log("curSpeed: " + curSpeed);
        // speed cannot go below 0 (SPEED, not VELOCITY)
        if (curSpeed < 0f) curSpeed = 0f;


        // set linear velocity to moveDir * curSpeed;
        rb.linearVelocity = vertMove + (moveDir * curSpeed);

        // lock angular velocity to 0;
        rb.angularVelocity = new();

        //Debug.Log("rb linearvelocity: " + rb.linearVelocity + ", vertMove" + vertMove + ", moveDir: " + moveDir + ", curSpeed: " + curSpeed);

    }
}
