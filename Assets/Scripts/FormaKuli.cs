using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Forma { Balon, Guma, Metal }

// Formy kuli: odblokowane przez podniesienie znajdźki, przełączane klawiszami 1/2/3
[RequireComponent(typeof(Rigidbody), typeof(KulaController))]
public class FormaKuli : MonoBehaviour
{
    public Forma startowa = Forma.Guma;

    public Forma Aktualna { get; private set; }
    private readonly HashSet<Forma> odblokowane = new HashSet<Forma>();

    private Rigidbody rb;
    private KulaController sterowanie;
    private Collider kolider;
    private Renderer rend;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sterowanie = GetComponent<KulaController>();
        kolider = GetComponent<Collider>();
        rend = GetComponent<Renderer>();

        odblokowane.Add(startowa);
        UstawForme(startowa);
    }

    void Update()
    {
        var k = Keyboard.current;
        if (k == null) return;
        if (k.digit1Key.wasPressedThisFrame) ZmienForme(Forma.Balon);
        if (k.digit2Key.wasPressedThisFrame) ZmienForme(Forma.Guma);
        if (k.digit3Key.wasPressedThisFrame) ZmienForme(Forma.Metal);
    }

    public bool CzyOdblokowana(Forma forma) => odblokowane.Contains(forma);

    // Wywoływane przez znajdźkę
    public void Odblokuj(Forma forma)
    {
        odblokowane.Add(forma);
        UstawForme(forma);
        Komunikat.Pokaz("Odblokowano: " + forma + "!");
    }

    private void ZmienForme(Forma forma)
    {
        if (odblokowane.Contains(forma) && forma != Aktualna)
        {
            UstawForme(forma);
            Komunikat.Pokaz("Forma: " + forma);
        }
    }

    public void UstawForme(Forma forma)
    {
        Aktualna = forma;
        var mat = new PhysicsMaterial(forma.ToString());

        switch (forma)
        {
            case Forma.Balon:
                rb.mass = 0.3f; rb.linearDamping = 1f; sterowanie.sila = 5f;
                mat.bounciness = 0.3f; mat.dynamicFriction = mat.staticFriction = 0.4f;
                rend.material.color = new Color(0.3f, 0.6f, 1f);
                break;

            case Forma.Guma:
                rb.mass = 1f; rb.linearDamping = 0.1f; sterowanie.sila = 15f;
                mat.bounciness = 0.8f; mat.dynamicFriction = mat.staticFriction = 0.8f;
                mat.bounceCombine = PhysicsMaterialCombine.Maximum;
                rend.material.color = new Color(0.9f, 0.2f, 0.2f);
                break;

            case Forma.Metal:
                rb.mass = 5f; rb.linearDamping = 0f; sterowanie.sila = 60f;
                mat.bounciness = 0f; mat.dynamicFriction = mat.staticFriction = 0.4f;
                mat.bounceCombine = PhysicsMaterialCombine.Minimum;
                rend.material.color = new Color(0.55f, 0.55f, 0.6f);
                break;
        }

        kolider.material = mat;
    }
}
