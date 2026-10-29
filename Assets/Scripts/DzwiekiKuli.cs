using UnityEngine;

// Dźwięki kuli: skok na trampolinie i upadek z toru
public class DzwiekiKuli : MonoBehaviour
{
    public AudioClip dzwiekSkoku;
    public AudioClip dzwiekUpadku;
    [Range(0f, 1f)] public float glosnosc = 0.8f;

    private AudioSource zrodlo;

    void Awake()
    {
        zrodlo = gameObject.AddComponent<AudioSource>();
        zrodlo.playOnAwake = false;
    }

    public void Skok()   { Zagraj(dzwiekSkoku); }
    public void Upadek() { Zagraj(dzwiekUpadku); }

    private void Zagraj(AudioClip clip)
    {
        if (clip != null) zrodlo.PlayOneShot(clip, glosnosc);
    }
}
