using UnityEngine;

// Niewidoczna strefa pod torem, po wpadnięciu kula wraca na checkpoint
public class StrefaUpadku : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var respawn = other.GetComponent<RespawnKuli>();
        if (respawn != null)
        {
            respawn.Respawn();

            var dzwieki = other.GetComponent<DzwiekiKuli>();
            if (dzwieki != null) dzwieki.Upadek();
            Komunikat.Pokaz("Spadłeś!");
        }
    }
}
