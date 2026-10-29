using UnityEngine;

// Znajdźka odblokowująca formę kuli, znika po podniesieniu
[RequireComponent(typeof(Collider))]
public class ZnajdzkaFormy : MonoBehaviour
{
    public Forma forma = Forma.Metal;
    public float predkoscObrotu = 90f;

    void Reset() => GetComponent<Collider>().isTrigger = true;

    void Update()
    {
        transform.Rotate(0, predkoscObrotu * Time.deltaTime, 0, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        var kula = other.GetComponent<FormaKuli>();
        if (kula == null) return;

        kula.Odblokuj(forma);
        Destroy(gameObject);
    }
}
