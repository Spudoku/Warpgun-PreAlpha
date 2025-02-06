using UnityEngine;
// Code based on this video: https://youtu.be/hiXYyn9NkOo
// by Solo Game Dev
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    public float speed = 5f;
    public float gravityScale = 0.5f;

    public float jumpStrength = 10f;

    private Vector3 vel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // general movement
        Vector3 movement = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // gravity!
        if (cc.isGrounded)
        {
            Debug.Log("Grounded!");
            vel.y = -2f;
            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("Attempting jump!");
                vel.y = jumpStrength;
            }

        }
        else
        {
            vel.y += gravityScale * Physics.gravity.y * Time.deltaTime;
        }

        cc.Move((movement * speed + vel) * Time.deltaTime);
    }
}
