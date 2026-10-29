using UnityEngine;

// Zapisuje miejsce odrodzenia i aktualną formę kuli
public class Checkpoint : MonoBehaviour
{
    public string napis = "";   // jeśli puste, bierze nazwę obiektu
    private bool zaliczony;

    void OnTriggerEnter(Collider other)
    {
        var respawn = other.GetComponent<RespawnKuli>();
        var forma = other.GetComponent<FormaKuli>();
        if (respawn == null || forma == null) return;

        respawn.UstawCheckpoint(transform.position + Vector3.up, forma.Aktualna);

        if (!zaliczony)
        {
            zaliczony = true;
            Komunikat.Pokaz(napis != "" ? napis : gameObject.name.Replace("Checkpoint", "Checkpoint "));
        }
    }
}
