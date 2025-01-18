using UnityEngine;
// Code based on this video: https://youtu.be/hiXYyn9NkOo
// by Solo Game Dev
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    public float speed = 5f;
    public float gravityScale = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // general movement
        Vector3 movement = new(Input.GetAxis("Horizontal"), Physics.gravity.y * gravityScale, Input.GetAxis("Vertical"));
        cc.Move(speed * Time.deltaTime * movement);
    }
}
