using UnityEngine;
[RequireComponent(typeof(VelBasedController))]
public class TeleportTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TeleportManager tm;
    [SerializeField] private float simpleDist;
    [SerializeField] private float smoothDist;
    [SerializeField] private float smoothDur;

    private VelBasedController vbc;

    void Start()
    {
        vbc = GetComponent<VelBasedController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // find destination
            Vector3 destination = transform.position + vbc.trueForward * simpleDist;
            Debug.Log("Attempting simple teleport from " + transform.position + " to " + destination);
            tm.SimpleTeleport(gameObject, destination);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Vector3 destination = transform.position + vbc.trueForward * smoothDist;
            Debug.Log("Attempting smooth teleport from " + transform.position + " to " + destination);
            StartCoroutine(tm.SmoothTeleport(gameObject, destination, smoothDur));

        }

    }
}
