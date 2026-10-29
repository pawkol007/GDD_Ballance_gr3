using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class KulaController : MonoBehaviour
{
    public float sila = 15f;      // siła pchania kuli
    public float mnoznikSterowania = 1f;  // np. na lodzie mniejszy
    public Transform kamera;      // jeśli puste, bierze Main Camera

    private Rigidbody rb;
    private Vector2 wejscie;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 50f; 
        if (kamera == null) kamera = Camera.main.transform;
    }

    void Update()
    {
        var k = Keyboard.current;
        if (k == null) return;

        wejscie = Vector2.zero;
        if (k.wKey.isPressed) wejscie.y += 1;
        if (k.sKey.isPressed) wejscie.y -= 1;
        if (k.dKey.isPressed) wejscie.x += 1;
        if (k.aKey.isPressed) wejscie.x -= 1;
    }

    void FixedUpdate()
    {
        // kierunki kamery 
        Vector3 przod = kamera.forward; przod.y = 0; przod.Normalize();
        Vector3 prawo = kamera.right;   prawo.y = 0; prawo.Normalize();

        Vector3 kierunek = (przod * wejscie.y + prawo * wejscie.x).normalized;
        rb.AddForce(kierunek * sila * mnoznikSterowania, ForceMode.Force);
    }
}
