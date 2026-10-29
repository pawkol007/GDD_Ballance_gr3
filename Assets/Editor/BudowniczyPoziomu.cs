using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Menu Ballance > Zbuduj poziom, buduje cały tor z prostych brył
public static class BudowniczyPoziomu
{
    static Transform rodzic;

    [MenuItem("Ballance/Zbuduj poziom")]
    public static void Zbuduj()
    {
        // usuń stary poziom, jeśli był
        var stary = GameObject.Find("Poziom");
        if (stary != null) Undo.DestroyObjectImmediate(stary);

        rodzic = new GameObject("Poziom").transform;
        Undo.RegisterCreatedObjectUndo(rodzic.gameObject, "Zbuduj poziom");
        rodzic.gameObject.AddComponent<Instrukcja>();

        var zwykly = Mat("Zwykly", new Color(0.75f, 0.72f, 0.65f));
        var lod    = Mat("Lod", new Color(0.7f, 0.9f, 1f));
        var szklo  = Mat("Szklo", new Color(0.55f, 0.85f, 0.85f));
        var tramp  = Mat("Trampolina", new Color(0.9f, 0.3f, 0.3f));
        var magnes = Mat("Magnes", new Color(0.35f, 0.35f, 0.45f));
        var check  = Mat("Checkpoint", new Color(0.3f, 0.8f, 0.3f));
        var meta   = Mat("Meta", new Color(1f, 0.85f, 0.2f));
        var ruch   = Mat("Ruchoma", new Color(1f, 0.55f, 0.1f));

        // CZĘŚĆ 1: guma
        Blok("Start", 0, 0, 0, 10, 10, zwykly);
        Blok("Sciezka", 0, 0, 13, 4, 16, zwykly);
        Blok("Zakret", 6, 0, 23, 16, 4, zwykly);
        Blok("Lod", 12, 0, 35, 4, 20, lod).AddComponent<Lod>();
        Blok("PrzedTrampolina", 12, 0, 48, 8, 6, zwykly);
        Blok("Trampolina1", new Vector3(12, 0.1f, 50), new Vector3(3, 0.2f, 2), tramp).AddComponent<Trampolina>();
        Blok("Gora1", 12, 3, 62, 8, 12, zwykly);
        Znajdzka(Forma.Balon, new Vector3(12, 3.6f, 64));
        CheckpointPad("Checkpoint1", new Vector3(12, 3, 59), check);

        // CZĘŚĆ 2: balon
        for (int i = 0; i < 8; i++)
            Blok("Szklo" + (i + 1), new Vector3(12, 3, 69 + 2 * i), new Vector3(2, 0.3f, 1.95f), szklo).AddComponent<KrucheSzklo>();
        Blok("PoMoscie", 12, 3, 88, 6, 8, zwykly);
        RuchomaNadPrzepascia(new Vector3(12, 2.75f, 93.5f), new Vector3(12, 2.75f, 104.5f), ruch);
        Blok("Wysepka", 12, 3, 110, 8, 8, zwykly);
        Znajdzka(Forma.Metal, new Vector3(12, 3.6f, 111));
        CheckpointPad("Checkpoint2", new Vector3(12, 3, 108), check);

        // CZĘŚĆ 3: metal
        Blok("Korytarz", 12, 3, 124, 4, 20, zwykly);
        WalecNaTorze(new Vector3(12, 3.6f, 131));
        Rampa("Magnes1", new Vector3(12, 3, 134), 10, 30, magnes);
        Blok("Gora2", 12, 8, 147.8f, 8, 10.4f, zwykly);
        CheckpointPad("Checkpoint3", new Vector3(12, 8, 146), check);

        // FINAŁ: szkło, trampolina, magnes
        for (int i = 0; i < 4; i++)
            Blok("FinalSzklo" + (i + 1), new Vector3(12, 8, 154 + 2 * i), new Vector3(3, 0.3f, 1.95f), szklo).AddComponent<KrucheSzklo>();
        Blok("FinalPlatforma", 12, 8, 164, 6, 6, zwykly);
        Blok("Trampolina2", new Vector3(12, 8.1f, 166), new Vector3(3, 0.2f, 2), tramp).AddComponent<Trampolina>();
        Blok("Gora3", 12, 11, 177, 8, 10, zwykly);
        Rampa("Magnes2", new Vector3(12, 11, 182), 8, 30, magnes);
        Blok("MetaPlatforma", 12, 15, 193, 8, 8, zwykly);
        var m = Blok("Meta", new Vector3(12, 15.05f, 195), new Vector3(4, 0.1f, 4), meta);
        m.AddComponent<Meta>();
        WysokiTrigger(m);

        // brama mety
        var brama = Mat("Brama", new Color(0.15f, 0.15f, 0.15f));
        Blok("BramaLewa", new Vector3(9.75f, 18.5f, 195), new Vector3(0.5f, 3.5f, 0.5f), brama);
        Blok("BramaPrawa", new Vector3(14.25f, 18.5f, 195), new Vector3(0.5f, 3.5f, 0.5f), brama);
        Blok("BramaGora", new Vector3(12, 19, 195), new Vector3(5, 0.5f, 0.5f), meta);

        // strefa upadku pod całym torem
        var strefa = Blok("StrefaUpadku", new Vector3(12, -8, 100), new Vector3(200, 1, 400), zwykly);
        strefa.GetComponent<Renderer>().enabled = false;
        strefa.GetComponent<Collider>().isTrigger = true;
        strefa.AddComponent<StrefaUpadku>();

        // ustaw kulę na starcie
        var kula = GameObject.Find("Kula");
        if (kula != null)
        {
            Undo.RecordObject(kula.transform, "Kula na start");
            kula.transform.position = new Vector3(0, 1, 0);
            if (kula.GetComponent<RespawnKuli>() == null) Undo.AddComponent<RespawnKuli>(kula);
        }
        else Debug.LogWarning("Nie znaleziono obiektu Kula na scenie");

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Poziom zbudowany");
    }

    // platforma: x, wysokość górnej powierzchni, z, szerokość, długość
    static GameObject Blok(string nazwa, float x, float gora, float z, float sx, float sz, Material m)
    {
        return Blok(nazwa, new Vector3(x, gora, z), new Vector3(sx, 0.5f, sz), m);
    }

    static GameObject Blok(string nazwa, Vector3 srodekGora, Vector3 rozmiar, Material m)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = nazwa;
        go.transform.SetParent(rodzic);
        go.transform.position = srodekGora - Vector3.up * rozmiar.y / 2f;
        go.transform.localScale = rozmiar;
        go.GetComponent<Renderer>().sharedMaterial = m;
        return go;
    }

    // pochylnia z płytą magnetyczną, start to początek górnej krawędzi
    static void Rampa(string nazwa, Vector3 start, float dlugosc, float kat, Material m)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = nazwa;
        go.transform.SetParent(rodzic);
        var obrot = Quaternion.Euler(-kat, 0, 0);
        go.transform.rotation = obrot;
        go.transform.localScale = new Vector3(4, 0.5f, dlugosc);
        go.transform.position = start + obrot * new Vector3(0, -0.25f, dlugosc / 2f);
        go.GetComponent<Renderer>().sharedMaterial = m;

        var trigger = go.AddComponent<BoxCollider>();
        trigger.isTrigger = true;
        trigger.size = new Vector3(1, 6, 1);
        trigger.center = new Vector3(0, 3, 0);
        go.AddComponent<PlytaMagnetyczna>();
    }

    // ruchoma platforma na skrypcie Filipa (RuchomaPlatforma)
    static void RuchomaNadPrzepascia(Vector3 a, Vector3 b, Material m)
    {
        var go = new GameObject("RuchomaPlatforma");
        go.transform.SetParent(rodzic);

        var punktA = new GameObject("PunktA").transform;
        punktA.SetParent(go.transform); punktA.position = a;
        var punktB = new GameObject("PunktB").transform;
        punktB.SetParent(go.transform); punktB.position = b;

        var platforma = Blok("Platforma", a + Vector3.up * 0.25f, new Vector3(3, 0.5f, 3), m);
        platforma.transform.SetParent(go.transform);
        platforma.AddComponent<PrzewozKuli>();

        var skrypt = go.AddComponent<RuchomaPlatforma>();
        var so = new SerializedObject(skrypt);
        so.FindProperty("PunktA").objectReferenceValue = punktA.gameObject;
        so.FindProperty("PunktB").objectReferenceValue = punktB.gameObject;
        so.FindProperty("platforma").objectReferenceValue = platforma;
        so.FindProperty("speed").floatValue = 3f;
        so.FindProperty("delay").floatValue = 1f;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    static void Znajdzka(Forma forma, Vector3 poz)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "Znajdzka_" + forma;
        go.transform.SetParent(rodzic);
        go.transform.position = poz;
        go.transform.localScale = Vector3.one * 0.5f;
        go.GetComponent<Collider>().isTrigger = true;
        go.GetComponent<Renderer>().sharedMaterial = forma == Forma.Balon
            ? Mat("ZnajdzkaBalon", new Color(0.3f, 0.6f, 1f))
            : Mat("ZnajdzkaMetal", new Color(0.55f, 0.55f, 0.6f));
        go.AddComponent<ZnajdzkaFormy>().forma = forma;
    }

    static void CheckpointPad(string nazwa, Vector3 poz, Material m)
    {
        var go = Blok(nazwa, poz + Vector3.up * 0.05f, new Vector3(2, 0.1f, 2), m);
        go.AddComponent<Checkpoint>();
        WysokiTrigger(go);
    }

    // płaski pad z triggerem sięgającym 2 m w górę
    static void WysokiTrigger(GameObject go)
    {
        var col = go.GetComponent<BoxCollider>();
        col.isTrigger = true;
        col.size = new Vector3(1, 20, 1);
        col.center = new Vector3(0, 10, 0);
    }

    static void WalecNaTorze(Vector3 poz)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        go.name = "Walec";
        go.transform.SetParent(rodzic);
        go.transform.position = poz;
        go.transform.rotation = Quaternion.Euler(0, 0, 90);
        go.transform.localScale = new Vector3(1.2f, 1.7f, 1.2f);
        go.GetComponent<Renderer>().sharedMaterial = Mat("Walec", new Color(0.5f, 0.35f, 0.2f));
        var rb = go.AddComponent<Rigidbody>();
        rb.mass = 3f;
        rb.linearDamping = 1f;
        go.AddComponent<Walec>();
    }

    // materiał zapisany jako plik, żeby nie zniknął po zamknięciu sceny
    static Material Mat(string nazwa, Color kolor)
    {
        if (!AssetDatabase.IsValidFolder("Assets/Materialy"))
            AssetDatabase.CreateFolder("Assets", "Materialy");

        string sciezka = "Assets/Materialy/" + nazwa + ".mat";
        var mat = AssetDatabase.LoadAssetAtPath<Material>(sciezka);
        if (mat == null)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            mat = new Material(shader);
            AssetDatabase.CreateAsset(mat, sciezka);
        }
        mat.color = kolor;
        EditorUtility.SetDirty(mat);
        return mat;
    }
}
