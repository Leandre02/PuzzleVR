using UnityEngine;
using TMPro;
using System;

/// <summary>
/// Gère l'affichage et la navigation entre tous les panneaux UI.
/// </summary>
public class GestionnaireCanvas : MonoBehaviour
{
    public static GestionnaireCanvas instance;

    [Header("Panneaux principaux")]
    [SerializeField] private GameObject panneauMenu;
    [SerializeField] private GameObject panneauJeu;
    [SerializeField] private GameObject panneauFin;

    [Header("Panneaux secondaires")]
    [SerializeField] private GameObject panneauInfos;       // ouvert par le coffre
    [SerializeField] private GameObject panneauControles;   // ouvert depuis le menu
    [SerializeField] private GameObject panneauCredentials; // ouvert depuis le panneau fin

    [Header("Textes UI")]
    [SerializeField] private TextMeshProUGUI texteTour;
    [SerializeField] private TextMeshProUGUI texteMinuterie;
    [SerializeField] private TextMeshProUGUI texteScoreFinal;

    void Awake()
    {
        instance = this;
        AfficherMenu();
    }

    void OnEnable()
    {
        GestionnaireJeu.onDebutPartie += AfficherJeu;
        GestionnaireJeu.onFinDePartie += AfficherFin;
    }

    void OnDisable()
    {
        GestionnaireJeu.onDebutPartie -= AfficherJeu;
        GestionnaireJeu.onFinDePartie -= AfficherFin;
    }

    /// <summary>
    /// Cache tous les panneaux d'un coup
    /// </summary>
    private void ToutCacher()
    {
        panneauMenu.SetActive(false);
        panneauJeu.SetActive(false);
        panneauFin.SetActive(false);
        panneauInfos.SetActive(false);
        panneauControles.SetActive(false);
        panneauCredentials.SetActive(false);
    }

    /// <summary>
    /// Affiche le menu principal et cache tous les autres panneaux
    /// </summary>
    public void AfficherMenu()
    {
        ToutCacher();
        panneauMenu.SetActive(true);
    }

    /// <summary>
    /// Affiche le panneau de jeu et cache tous les autres panneaux
    /// </summary>
    private void AfficherJeu()
    {
        ToutCacher();
        panneauJeu.SetActive(true);
    }

    /// <summary>
    /// Affiche le panneau de fin et cache tous les autres panneaux, en affichant le score final (nombre de tours complétés) dans le texte dédié
    /// </summary>
    /// <param name="toursCompletes">Le nombre de tours complétés par le joueur</param>
    private void AfficherFin(int toursCompletes)
    {
        ToutCacher();
        panneauFin.SetActive(true);
        texteScoreFinal.text = "Tours complétés : " + toursCompletes;
    }

    /// <summary>
    /// Méthodes pour afficher les panneaux d'infos, de contrôles et de credentials.
    /// </summary>
    public void AfficherInfos()
    {
        ToutCacher();
        panneauInfos.SetActive(true);
    }

    /// <summary>
    /// Ferme le panneau d'informations et affiche le menu principal.   
    /// </summary>
    /// <remarks>Utilisez cette méthode pour masquer l'interface d'informations et revenir à l'écran du menu
    /// principal. Cette méthode n'a aucun effet si le panneau d'informations est déjà fermé.</remarks>
    public void FermerInfos()
    {
        panneauInfos.SetActive(false);
        panneauMenu.SetActive(true); // retourne au menu
    }

    /// <summary>
    /// Affiche le panneau des contrôles à l'utilisateur.
    /// </summary>
    /// <remarks>Utilisez cette méthode pour rendre visible le panneau des contrôles après l'avoir masqué.
    /// Cette méthode garantit que seuls les contrôles sont affichés, masquant les autres éléments d'interface si
    /// nécessaire.</remarks>
    public void AfficherControles()
    {
        ToutCacher();
        panneauControles.SetActive(true);
    }

    /// <summary>
    /// Ferme le panneau de contrôles et affiche le menu principal.
    /// </summary>
    /// <remarks>Utilisez cette méthode pour masquer l'interface de contrôles et revenir à l'écran du menu
    /// principal. Cette opération modifie l'état d'affichage des panneaux associés.</remarks>
    public void FermerControles()
    {
        panneauControles.SetActive(false);
        panneauMenu.SetActive(true); // retourne au menu
    }

    /// <summary>
    /// Affiche le panneau des informations d'identification en masquant tous les autres panneaux.
    /// </summary>
    /// <remarks>Utilisez cette méthode pour rendre visible uniquement le panneau des informations
    /// d'identification à l'écran. Tous les autres panneaux seront masqués lors de l'appel de cette méthode.</remarks>
    public void AfficherCredentials()
    {
        ToutCacher();
        panneauCredentials.SetActive(true);
    }

    /// <summary>
    /// Masque le panneau d'identification et affiche le panneau de fin de processus.
    /// </summary>
    /// <remarks>Utiliser cette méthode pour terminer la saisie des informations d'identification et passer à
    /// l'étape finale de l'application. Cette méthode modifie l'état d'affichage des panneaux associés à l'interface
    /// utilisateur.</remarks>
    public void FermerCredentials()
    {
        panneauCredentials.SetActive(false);
        panneauFin.SetActive(true); // retourne au panneau de fin
    }

    /// <summary>
    /// Mise à jour du HUD pendant le jeu
    /// </summary>
    public void MettreAJourHUD(int tour, float tempsRestant)
    {
        texteTour.text = "Tour " + tour;
        texteMinuterie.text = Mathf.CeilToInt(tempsRestant).ToString();
    }
}