using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Fait osciller le coffre pour attirer l'attention au menu.
/// Ouvre le panneau d'infos quand le joueur le grab.
/// </summary>
public class CoffreInfo : MonoBehaviour
{
    [SerializeField] private float amplitudeVibration = 0.3f;

    private XRGrabInteractable grab;
    private Vector3 positionDepart;
    private bool partieEnCours = false;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        positionDepart = transform.position;
    }

    void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrab);
        GestionnaireJeu.onDebutPartie += OnDebutPartie;
        GestionnaireJeu.onFinDePartie += OnFinDePartie;
    }

    void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        GestionnaireJeu.onDebutPartie -= OnDebutPartie;
        GestionnaireJeu.onFinDePartie -= OnFinDePartie;
    }

    /// <summary>
    /// Permet d'osciller le coffre de haut en bas pour attirer l'attention du joueur sur le coffre et les infos avant de commencer la partie.
    /// Code généré par chatgpt modele 5.5, Avril 2026
    /// </summary>
    void Update()
    {
        if (!partieEnCours)
        {
            transform.position = positionDepart + Vector3.up * Mathf.Sin(Time.time * 3f) * 0.05f;
        }
    }

    // Fin code généré par Chatgpt

    /// <summary>
    /// Affiche le panneau d'infos et fait vibrer le contrôleur quand le coffre est grab, pour attirer l'attention du joueur sur les infos avant de commencer la partie.
    /// S'inspire de l'exercice sur les feedbacks VR (haptiques et audio spatial) et de l'utilisation du XR Grab Interactable / événements
    /// du XR Interaction Toolkit (selectEntered / selectExited).
    /// Références :
    ///  - Cégep de Victoriaville. Exercice 4.1 — Feedback VR : haptiques et audio spatial. Environnements Immersifs, 2026. https://envimmersif-cegepvicto.github.io/exercice_feedback_vr/
    /// </summary>
    /// <param name="args"></param>
    private void OnGrab(SelectEnterEventArgs args)
    {
        GestionnaireCanvas.instance.AfficherInfos();

        var controller = args.interactorObject.transform.GetComponent<XRBaseInputInteractor>();
        if (controller != null)
        {
            controller.SendHapticImpulse(amplitudeVibration, 0.2f);
        }
    }

    /// <summary>
    /// Initialise l'état de la partie au début d'une session de jeu.
    /// </summary>
    private void OnDebutPartie()
    {
        partieEnCours = true;
        grab.enabled = false;
    }

    /// <summary>
    /// Initialise l'état de la partie à la fin d'une session de jeu, réactivant le grab pour permettre au joueur de consulter les infos avant de recommencer une partie.
    /// </summary>
    /// <param name="toursCompletes">Le nombre de tours complétés lors de la partie.</param>
    private void OnFinDePartie(int toursCompletes)
    {
        partieEnCours = false;
        grab.enabled = true;
    }
}