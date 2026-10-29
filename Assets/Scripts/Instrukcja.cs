using UnityEngine;
using UnityEngine.InputSystem;

// Krótka instrukcja na starcie, gra rusza po wciśnięciu spacji
public class Instrukcja : MonoBehaviour
{
    private static bool pokazana; // tylko przy pierwszym uruchomieniu, nie po restarcie
    private bool widoczna;

    void Start()
    {
        if (pokazana) return;
        widoczna = true;
        Time.timeScale = 0f;
    }

    void Update()
    {
        var k = Keyboard.current;
        if (widoczna && k != null && k.spaceKey.wasPressedThisFrame)
        {
            widoczna = false;
            pokazana = true;
            Time.timeScale = 1f;
        }
    }

    void OnGUI()
    {
        if (!widoczna) return;

        float w = 620, h = 330;
        var okno = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);
        GUI.Box(okno, "");
        GUI.Box(okno, "");

        var styl = new GUIStyle(GUI.skin.label) { fontSize = 24, alignment = TextAnchor.MiddleCenter };
        var tytul = new GUIStyle(styl) { fontSize = 36, fontStyle = FontStyle.Bold };

        string[] linie =
        {
            "WASD: ruch   Mysz: kamera   1/2/3: zmiana formy",
            "Balon: lekki, przejedzie po kruchym szkle",
            "Guma: sprężysta, wybija się na trampolinie",
            "Metal: ciężki, trzyma się płyty magnetycznej",
            "Formy odblokowujesz, zbierając znajdźki"
        };

        GUI.Label(new Rect(okno.x, okno.y + 15, w, 50), "Jak grać", tytul);
        for (int i = 0; i < linie.Length; i++)
            GUI.Label(new Rect(okno.x, okno.y + 75 + i * 38, w, 36), linie[i], styl);
        GUI.Label(new Rect(okno.x, okno.y + h - 50, w, 40), "Wciśnij SPACJĘ, aby zacząć", tytul);
    }
}
