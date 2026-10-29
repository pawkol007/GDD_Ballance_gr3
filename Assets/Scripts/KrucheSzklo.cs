using UnityEngine;

// Płytka pęka pod ciężką kulą i po chwili się odnawia
public class KrucheSzklo : MonoBehaviour
{
    public float maxMasa = 0.5f;    // balon (0.3) przejedzie, guma i metal nie
    public float czasOdnowienia = 3f;

    private Renderer rend;
    private Collider kolider;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        kolider = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.rigidbody != null && c.rigidbody.mass > maxMasa)
            Invoke(nameof(Pekaj), 0.1f);
    }

    void Pekaj()
    {
        rend.enabled = false;
        kolider.enabled = false;
        Invoke(nameof(Odnow), czasOdnowienia);
    }

    void Odnow()
    {
        rend.enabled = true;
        kolider.enabled = true;
    }
}
