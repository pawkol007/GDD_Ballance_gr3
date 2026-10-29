using UnityEngine;

// Wybija kulę w górę, mocno tylko gumę
public class Trampolina : MonoBehaviour
{
    public float silaWybicia = 11f;
    public float mnoznikInnych = 0.3f; // balon i metal prawie się nie odbijają

    void OnCollisionEnter(Collision c)
    {
        var forma = c.collider.GetComponent<FormaKuli>();
        if (forma == null || c.rigidbody == null) return;

        float mnoznik = forma.Aktualna == Forma.Guma ? 1f : mnoznikInnych;
        Vector3 v = c.rigidbody.linearVelocity;
        v.y = silaWybicia * mnoznik;
        c.rigidbody.linearVelocity = v;

        var dzwieki = c.collider.GetComponent<DzwiekiKuli>();
        if (dzwieki != null) dzwieki.Skok();
    }
}
