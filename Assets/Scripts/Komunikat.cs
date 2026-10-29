using UnityEngine;

// Napis na środku ekranu, np. "Checkpoint 1" albo "Forma: Metal"
public class Komunikat : MonoBehaviour
{
    private static Komunikat instancja;
    private string tekst;
    private float czasDo;

    public static void Pokaz(string nowyTekst, float czas = 2f)
    {
        if (instancja == null)
            instancja = new GameObject("Komunikat").AddComponent<Komunikat>();

        instancja.tekst = nowyTekst;
        instancja.czasDo = Time.time + czas;
    }

    void OnGUI()
    {
        if (Time.time > czasDo) return;

        var styl = new GUIStyle(GUI.skin.label)
        {
            fontSize = 40,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        var prostokat = new Rect(0, Screen.height * 0.2f, Screen.width, 60);

        // cień pod napisem, żeby był czytelny
        GUI.color = Color.black;
        GUI.Label(new Rect(prostokat.x + 2, prostokat.y + 2, prostokat.width, prostokat.height), tekst, styl);
        GUI.color = Color.white;
        GUI.Label(prostokat, tekst, styl);
    }
}
