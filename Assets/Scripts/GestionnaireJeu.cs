using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Classe centrale pour gérer le déroulement du jeu : démarrage, fin, score, minuterie, et transitions entre les différents panneaux UI
/// S'appuie sur les patterns de GameManager et d'UI en World Space vus dans les notes et exercices (UI VR, GameManager)
/// et sur les consignes du travail pratique Whack-a-Mole VR.
/// Références :
///  - Cégep de Victoriaville. UI en VR. Environnements Immersifs, 2026.
///  - Cégep de Victoriaville. Exercice 4.2 — UI VR et GameManager. Environnements Immersifs, 2026.
///  - Cégep de Victoriaville. Travail pratique — Whack-a-Mole VR. Environnements Immersifs, 2026.
/// </summary>
public class GestionnaireJeu : MonoBehaviour
{
    // Singleton de GestionnaireJeu pour permettre à d'autres scripts d'y accéder facilement
    public static GestionnaireJeu instance;

    [Header("Durée")]
    [SerializeField] private float dureeInitiale = 60f;
    [SerializeField] private float bonusParTour = 15f;

    [Header("UI - Panneau Jeu")]
    [SerializeField] private TextMeshProUGUI texteTour;
    [SerializeField] private TextMeshProUGUI texteMinuterie;

    [Header("UI - Panneaux")]
    [SerializeField] private GameObject panneauMenu;
    [SerializeField] private GameObject panneauJeu;
    [SerializeField] private GameObject panneauFin;

    [Header("UI - Fin de partie")]
    [SerializeField] private TextMeshProUGUI texteScoreFinal;

    [Header("Références")]
    [SerializeField] private GestionnaireSpawn gestionnaireSpawn;
    [SerializeField] private GestionnairePuzzle gestionnairePuzzle;

    [Header("Contrôleurs pour vibration")]
    [SerializeField] private XRBaseInputInteractor controleurGauche;
    [SerializeField] private XRBaseInputInteractor controleurDroit;

    private int numeroTour = 1;
    private float tempsRestant;
    private bool partieEnCours = false;

    void Awake()
    {
        instance = this;

        // État initial — on montre le menu
        panneauMenu.SetActive(true);
        panneauJeu.SetActive(false);
        panneauFin.SetActive(false);
    }

    /// <summary>
    /// Méthode pour démarrer la partie, appelée par le bouton "Jouer" du menu
    /// </summary>
    public void CommencerPartie()
    {
        numeroTour = 1;
        tempsRestant = dureeInitiale;
        partieEnCours = true;

        panneauMenu.SetActive(false);
        panneauJeu.SetActive(true);
        panneauFin.SetActive(false);

        gestionnaireSpawn.DemarrerSpawn();
        MettreAJourUI();
    }

    void Update()
    {
        if (!partieEnCours) return;

        tempsRestant -= Time.deltaTime;
        tempsRestant = Mathf.Max(0, tempsRestant);
        texteMinuterie.text = Mathf.CeilToInt(tempsRestant).ToString();

        if (tempsRestant <= 0)
        {
            FinDePartie();
        }
    }

    /// <summary>
    /// Appelée par GestionnairePuzzle quand toutes les pièces sont placées.
    /// Lance un nouveau tour avec un bonus de temps.
    /// </summary>
    public void TourComplete()
    {
        if (!partieEnCours) return;

        // Petite vibration de succès sur les deux contrôleurs
        if (controleurGauche != null)
        {
            controleurGauche.SendHapticImpulse(0.6f, 0.15f);
        }
        if (controleurDroit != null)
        {
            controleurDroit.SendHapticImpulse(0.6f, 0.15f);
        }

        // Passage au tour suivant
        numeroTour++;
        tempsRestant += bonusParTour;

        // Reset du puzzle pour un nouveau tour
        ResetPuzzle();
        MettreAJourUI();
    }

    /// <summary>
    /// Appelée quand le temps atteint zéro. Termine la partie et affiche le score final.
    /// </summary>
    void FinDePartie()
    {
        partieEnCours = false;
        gestionnaireSpawn.ArreterSpawn();

        // Vibration de fin longue sur les deux contrôleurs
        if (controleurGauche != null)
        {
            controleurGauche.SendHapticImpulse(1f, 0.5f);
        }
        if (controleurDroit != null)
        {
            controleurDroit.SendHapticImpulse(1f, 0.5f);
        }

        // Affichage du score final
        panneauJeu.SetActive(false);
        panneauFin.SetActive(true);

        int toursCompletes = numeroTour - 1;
        texteScoreFinal.text = "Tours complétés : " + toursCompletes;
    }

    /// <summary>
    /// Permet de recommencer une nouvelle partie, appelée par le bouton "Rejouer" du panneau de fin
    /// </summary>
    public void NouvellePartie()
    {
        ResetPuzzle();
        CommencerPartie();
    }

    /// <summary>
    /// Met à jour l'affichage du tour et du temps restant dans l'UI
    /// </summary>
    void MettreAJourUI()
    {
        texteTour.text = "Tour " + numeroTour;
        texteMinuterie.text = Mathf.CeilToInt(tempsRestant).ToString();
    }

    /// <summary>
    /// Réinitialise le puzzle, détruit les pièces actuelles et demande un nouveau spawn
    /// </summary>
    void ResetPuzzle()
    {
        // Détruit les pièces existantes
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (GameObject piece in pieces)
        {
            Destroy(piece);
        }

        // Réinitialise le compteur de sockets du puzzle
        if (gestionnairePuzzle != null)
        {
            gestionnairePuzzle.Reset();
        }

        // Relance un spawn pour le nouveau tour
        gestionnaireSpawn.DemarrerSpawn();
    }
}