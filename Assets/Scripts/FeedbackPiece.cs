using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Classe pour gérer les feedbacks haptiques sur une piece du puzzle lors de son grab et de son placement dans un socket.
/// S’inspire de l’exercice sur les feedbacks VR (haptiques et audio spatial) et de l’utilisation du XR Grab Interactable / événements
/// du XR Interaction Toolkit (selectEntered / selectExited).
/// Références :
///  - Cégep de Victoriaville. Exercice 4.1 — Feedback VR : haptiques et audio spatial. Environnements Immersifs, 2026. https://envimmersif-cegepvicto.github.io/exercice_feedback_vr/
///  - Cégep de Victoriaville. Exercice 4 — Tri spatial VR : Grab & Socket. Environnements Immersifs, 2026. https://envimmersif-cegepvicto.github.io/exercice_tri_vr/
///  - Unity Technologies. XR Interaction Toolkit Documentation. https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.0/manual/upgrade-guide-3.0.html
/// </summary>
public class FeedbackPiece : MonoBehaviour
{
    [Header("Haptique - Grab")]
    [SerializeField] private float amplitudeGrab = 0.5f;
    [SerializeField] private float dureeGrab = 0.1f;

    [Header("Audio")]
    [SerializeField] private AudioClip sonSpawn;

    private XRGrabInteractable grabInteractable;
    private XRBaseInputInteractor controller; // Nouveau component du XR interaction Toolkit
    private AudioSource audioSource;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();

        audioSource.spatialBlend = 1f;
    }

    void Start()
    {
        audioSource.PlayOneShot(sonSpawn); // joue au moment du spawn
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelache);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelache);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Garde une référence au contrôleur
        controller = args.interactorObject.transform.GetComponent<XRBaseInputInteractor>();

        if (controller != null)
        {
            controller.SendHapticImpulse(amplitudeGrab, dureeGrab); //  vibration Haptique
        }
    }

    private void OnRelache(SelectExitEventArgs args)
    {
        controller = null;
    }
}