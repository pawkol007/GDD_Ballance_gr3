using UnityEngine;
using UnityEngine.InputSystem;

public class KameraKuli : MonoBehaviour
{
    public Transform cel;               // kula
    public float odleglosc = 6f;
    public float wysokosc = 3f;
    public float predkoscObrotu = 120f; // stopnie na sekundę
    public float wygladzanie = 8f;

    private float kat;

    void LateUpdate()
    {
        if (cel == null) return;

        var k = Keyboard.current;
        if (k != null)
        {
            if (k.leftArrowKey.isPressed)  kat -= predkoscObrotu * Time.deltaTime;
            if (k.rightArrowKey.isPressed) kat += predkoscObrotu * Time.deltaTime;
        }

        Vector3 docelowa = cel.position + Quaternion.Euler(0, kat, 0) * new Vector3(0, wysokosc, -odleglosc);
        transform.position = Vector3.Lerp(transform.position, docelowa, wygladzanie * Time.deltaTime);
        transform.LookAt(cel.position + Vector3.up * 0.5f);
    }
}
