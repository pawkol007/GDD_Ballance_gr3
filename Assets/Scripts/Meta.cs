using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Koniec poziomu: zatrzymuje kulę, pokazuje czas i rekord, R zaczyna od nowa
public class Meta : MonoBehaviour
{
    private bool koniec;
    private float czas;
    private float rekord;
    private bool nowyRekord;
    private RespawnKuli kula;

    void Start() => kula = FindAnyObjectByType<RespawnKuli>();

    void OnTriggerEnter(Collider other)
    {
        if (koniec || other.GetComponent<FormaKuli>() == null) return;
        koniec = true;
        czas = Time.timeSinceLevelLoad;

        // zatrzymanie kuli i wyłączenie sterowania
        var rb = other.attachedRigidbody;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        other.GetComponent<KulaController>().enabled = false;
        other.GetComponent<FormaKuli>().enabled = false;

        // najlepszy czas zapisany na komputerze
        rekord = PlayerPrefs.GetFloat("Rekord", 0f);
        if (rekord == 0f || czas < rekord)
        {
            rekord = czas;
            nowyRekord = true;
            PlayerPrefs.SetFloat("Rekord", rekord);
            PlayerPrefs.Save();
        }
    }

    void Update()
    {
        var k = Keyboard.current;
        if (koniec && k != null && k.rKey.wasPressedThisFrame)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private int LiczbaSmierci() => kula != null ? kula.Smierci : 0;

    void OnGUI()
    {
        var styl = new GUIStyle(GUI.skin.label) { fontSize = 28 };

        if (!koniec)
        {
            GUI.Label(new Rect(20, 20, 600, 50), "Czas: " + Time.timeSinceLevelLoad.ToString("0.0") + " s", styl);
            GUI.Label(new Rect(20, 60, 600, 50), "Śmierci: " + LiczbaSmierci(), styl);
            return;
        }

        // okienko z wynikiem na środku ekranu
        float w = 500, h = 300;
        var okno = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);
        GUI.Box(okno, "");
        GUI.Box(okno, "");

        var srodek = new GUIStyle(styl) { alignment = TextAnchor.MiddleCenter };
        var duzy = new GUIStyle(srodek) { fontSize = 48, fontStyle = FontStyle.Bold };

        GUI.Label(new Rect(okno.x, okno.y + 20, w, 60), "META!", duzy);
        GUI.Label(new Rect(okno.x, okno.y + 90, w, 40), "Twój czas: " + czas.ToString("0.00") + " s", srodek);
        GUI.Label(new Rect(okno.x, okno.y + 130, w, 40),
            nowyRekord ? "Nowy rekord!" : "Rekord: " + rekord.ToString("0.00") + " s", srodek);
        GUI.Label(new Rect(okno.x, okno.y + 170, w, 40), "Śmierci: " + LiczbaSmierci(), srodek);
        GUI.Label(new Rect(okno.x, okno.y + 230, w, 40), "Wciśnij R, aby zagrać ponownie", srodek);
    }
}
