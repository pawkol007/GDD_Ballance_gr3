using UnityEngine;

// Stroma, śliska płyta: metal jest przyciągany, reszta się zsuwa
public class PlytaMagnetyczna : MonoBehaviour
{
    public float przyciaganie = 15f;
    public float silaZsuwania = 25f;

    void OnTriggerStay(Collider other)
    {
        var forma = other.GetComponent<FormaKuli>();
        var rb = other.attachedRigidbody;
        if (forma == null || rb == null) return;

        if (forma.Aktualna == Forma.Metal)
        {
            rb.AddForce(-transform.up * przyciaganie, ForceMode.Acceleration);
        }
        else
        {
            Vector3 wDol = Vector3.ProjectOnPlane(Vector3.down, transform.up).normalized;
            rb.AddForce(wDol * silaZsuwania, ForceMode.Acceleration);
        }
    }
}
