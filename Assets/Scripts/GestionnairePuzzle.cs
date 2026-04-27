using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Classe pour détecter quand toutes les pièces du puzzle sont placées dans les bons sockets.
/// S'inspire des exercices de tri spatial VR qui utilisent les événements selectEntered des sockets.
/// Références :
///  - Cégep de Victoriaville. Exercice 4 — Tri spatial VR : Grab & Socket. Environnements Immersifs, 2026.
/// </summary>
public class GestionnairePuzzle : MonoBehaviour
{
    [Header("Sockets de la maquette")]
    [SerializeField] private XRSocketInteractor[] sockets;

    private int nbSocketsRemplis = 0;

    void OnEnable()
    {
        // S'abonne aux événements de chaque socket
        foreach (XRSocketInteractor socket in sockets)
        {
            socket.selectEntered.AddListener(OnPiecePlacee);
            socket.selectExited.AddListener(OnPieceRetiree);
        }
    }

    void OnDisable()
    {
        // Se désabonne pour éviter les fuites mémoire
        foreach (XRSocketInteractor socket in sockets)
        {
            socket.selectEntered.RemoveListener(OnPiecePlacee);
            socket.selectExited.RemoveListener(OnPieceRetiree);
        }
    }

    private void OnPiecePlacee(SelectEnterEventArgs args)
    {
        nbSocketsRemplis++;

        // Vérifie si tous les sockets sont remplis
        if (nbSocketsRemplis >= sockets.Length)
        {
            Victoire();
        }
    }

    private void OnPieceRetiree(SelectExitEventArgs args)
    {
        // Si le joueur reprend une pièce déjà placée
        nbSocketsRemplis--;
    }

    /// <summary>
    /// Déclenche la complétion du tour via le GestionnaireJeu
    /// </summary>
    void Victoire()
    {
        if (GestionnaireJeu.instance != null)
        {
            GestionnaireJeu.instance.TourComplete();
        }
    }

    /// <summary>
    /// Réinitialise le compteur de sockets remplis. Appelée au début de chaque nouveau tour.
    /// </summary>
    public void Reset()
    {
        nbSocketsRemplis = 0;
    }
}