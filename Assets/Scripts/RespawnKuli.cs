using UnityEngine;

// Pamięta ostatni checkpoint i przywraca tam kulę po upadku
public class RespawnKuli : MonoBehaviour
{
    public int Smierci { get; private set; }

    private Vector3 punkt;
    private Forma forma;
    private Rigidbody rb;
    private FormaKuli formaKuli;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        formaKuli = GetComponent<FormaKuli>();
        punkt = transform.position;
        forma = formaKuli.Aktualna;
    }

    public void UstawCheckpoint(Vector3 nowyPunkt, Forma nowaForma)
    {
        punkt = nowyPunkt;
        forma = nowaForma;
    }

    public void Respawn()
    {
        Smierci++;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = punkt;
        formaKuli.UstawForme(forma);
    }
}
