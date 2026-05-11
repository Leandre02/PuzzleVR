using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Classe centrale pour gérer le déroulement du jeu : démarrage, fin, score, minuterie.
/// La gestion de l'UI est déléguée à GestionnaireCanvas.
/// Références :
///  - Cégep de Victoriaville. UI en VR. Environnements Immersifs, 2026.
///  - Cégep de Victoriaville. Exercice 4.2 — UI VR et GameManager. Environnements Immersifs, 2026.
///  - Cégep de Victoriaville. Travail pratique — Whack-a-Mole VR. Environnements Immersifs, 2026.
/// </summary>
public class GestionnaireJeu : MonoBehaviour
{
    public static GestionnaireJeu instance;

    [Header("Durée")]
    [SerializeField] private float dureeInitiale = 60f;
    [SerializeField] private float bonusParTour = 15f;

    [Header("Références")]
    [SerializeField] private GestionnaireSpawn gestionnaireSpawn;
    [SerializeField] private GestionnairePuzzle gestionnairePuzzle;

    [Header("Contrôleurs pour vibration")]
    [SerializeField] private XRBaseInputInteractor controleurGauche;
    [SerializeField] private XRBaseInputInteractor controleurDroit;

    private int numeroTour = 1;
    private float tempsRestant;
    private bool partieEnCours = false;

    public static event Action onDebutPartie;
    public static event Action<int> onFinDePartie;

    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Méthode pour démarrer la partie, appelée par le bouton "Jouer" du menu
    /// </summary>
    public void CommencerPartie()
    {
        numeroTour = 1;
        tempsRestant = dureeInitiale;
        partieEnCours = true;

        onDebutPartie?.Invoke();
        GestionnaireCanvas.instance.MettreAJourHUD(numeroTour, tempsRestant);

        gestionnaireSpawn.DemarrerSpawn();
    }

    void Update()
    {
        if (!partieEnCours) return;

        tempsRestant -= Time.deltaTime;
        tempsRestant = Mathf.Max(0, tempsRestant);

        GestionnaireCanvas.instance.MettreAJourHUD(numeroTour, tempsRestant);

        if (tempsRestant <= 0)
        {
            FinDePartie();
        }
    }

    /// <summary>
    /// Appelée par GestionnairePuzzle quand toutes les pièces sont placées.
    /// </summary>
    public void TourComplete()
    {
        if (!partieEnCours) return;

        if (controleurGauche != null) controleurGauche.SendHapticImpulse(0.6f, 0.15f);
        if (controleurDroit != null) controleurDroit.SendHapticImpulse(0.6f, 0.15f);

        numeroTour++;
        tempsRestant += bonusParTour;

        ResetPuzzle();
        GestionnaireCanvas.instance.MettreAJourHUD(numeroTour, tempsRestant);
    }

    /// <summary>
    /// Termine la partie quand le temps atteint zéro.
    /// </summary>
    void FinDePartie()
    {
        partieEnCours = false;

        // Désactive le grab sur les pièces restantes
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (GameObject piece in pieces)
        {
            piece.GetComponent<XRGrabInteractable>().enabled = false;
        }

        gestionnaireSpawn.ArreterSpawn();

        if (controleurGauche != null) controleurGauche.SendHapticImpulse(1f, 0.5f);
        if (controleurDroit != null) controleurDroit.SendHapticImpulse(1f, 0.5f);

        onFinDePartie?.Invoke(numeroTour - 1);
    }

    /// <summary>
    /// Recommence une nouvelle partie, appelée par le bouton "Rejouer"
    /// </summary>
    public void NouvellePartie()
    {
        ResetPuzzle();
        CommencerPartie();
    }

    /// <summary>
    /// Réinitialise le puzzle, détruit les pièces actuelles et demande un nouveau spawn
    /// </summary>
    void ResetPuzzle()
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (GameObject piece in pieces)
        {
            Destroy(piece);
        }

        if (gestionnairePuzzle != null)
        {
            gestionnairePuzzle.Reset();
        }

        gestionnaireSpawn.DemarrerSpawn();
    }
}