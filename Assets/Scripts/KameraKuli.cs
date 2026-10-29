using UnityEngine;
using UnityEngine.InputSystem;

public class KameraKuli : MonoBehaviour
{
    public Transform cel;
    public float odleglosc = 6f;
    public float czuloscMyszki = 0.5f;

    // Ograniczenia wychylenia kamery w pionie (żeby nie wejść pod mapę lub nie zrobić salta)
    public Vector2 limityY = new Vector2(-15f, 60f);

    private float rotacjaX; // obrót lewo/prawo
    private float rotacjaY; // obrót góra/dół

    void Start()
    {
        // Blokuje kursor myszy na środku ekranu i go ukrywa
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (cel == null) return;

        var m = Mouse.current;
        if (m != null)
        {
            Vector2 ruchMyszki = m.delta.ReadValue();

            rotacjaX += ruchMyszki.x * czuloscMyszki;
            rotacjaY -= ruchMyszki.y * czuloscMyszki; // Minus odwraca oś Y, by sterowanie było naturalne

            // Ograniczamy obrót góra/dół, żeby kamera nie przeszła przez bieguny
            rotacjaY = Mathf.Clamp(rotacjaY, limityY.x, limityY.y);
        }

        // Obliczamy pozycję na sferze wokół kuli
        Quaternion obrot = Quaternion.Euler(rotacjaY, rotacjaX, 0);
        Vector3 docelowa = cel.position + obrot * new Vector3(0, 0, -odleglosc);

        transform.position = docelowa;
        transform.LookAt(cel.position);
    }
}