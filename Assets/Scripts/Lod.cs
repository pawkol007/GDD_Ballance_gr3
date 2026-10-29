using UnityEngine;

// Na lodzie kula słabo reaguje na sterowanie i trudno ją wyhamować
public class Lod : MonoBehaviour
{
    public float mnoznik = 0.3f;

    void OnCollisionEnter(Collision c)
    {
        var kula = c.collider.GetComponent<KulaController>();
        if (kula != null) kula.mnoznikSterowania = mnoznik;
    }

    void OnCollisionExit(Collision c)
    {
        var kula = c.collider.GetComponent<KulaController>();
        if (kula != null) kula.mnoznikSterowania = 1f;
    }
}
