using UnityEngine;

// Przesuwa kulę razem z ruchomą platformą, żeby z niej nie zjeżdżała
public class PrzewozKuli : MonoBehaviour
{
    private Vector3 ostatniaPozycja;
    private Rigidbody kula;

    void Start() => ostatniaPozycja = transform.position;

    void FixedUpdate()
    {
        Vector3 przesuniecie = transform.position - ostatniaPozycja;
        if (kula != null) kula.MovePosition(kula.position + przesuniecie);
        ostatniaPozycja = transform.position;
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.collider.GetComponent<FormaKuli>() != null) kula = c.rigidbody;
    }

    void OnCollisionExit(Collision c)
    {
        if (c.rigidbody == kula) kula = null;
    }
}
