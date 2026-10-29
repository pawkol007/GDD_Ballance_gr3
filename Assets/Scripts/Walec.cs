using UnityEngine;

// Walec toczy się w stronę gracza, lekką kulę spycha, metal go przepycha
[RequireComponent(typeof(Rigidbody))]
public class Walec : MonoBehaviour
{
    public Vector3 kierunek = Vector3.back;
    public float sila = 20f;

    private Rigidbody rb;
    private Vector3 start;
    private Quaternion startObrot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        start = transform.position;
        startObrot = transform.rotation;
    }

    void FixedUpdate()
    {
        rb.AddForce(kierunek.normalized * sila, ForceMode.Force);

        // spadł z toru, wraca na początek
        if (transform.position.y < start.y - 10f)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.SetPositionAndRotation(start, startObrot);
        }
    }
}
