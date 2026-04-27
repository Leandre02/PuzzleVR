using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Une classe permettant de masquer la maquette quand le socket recoit une piece
/// </summary>
public class VisuelSocket : MonoBehaviour
{
    [SerializeField] private GameObject meshFantome;

    private XRSocketInteractor socket;

    void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    void OnEnable()
    {
        socket.selectEntered.AddListener(OnPiecePlacee);
        socket.selectExited.AddListener(OnPieceRetiree);
    }

    void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnPiecePlacee);
        socket.selectExited.RemoveListener(OnPieceRetiree);
    }

    private void OnPiecePlacee(SelectEnterEventArgs args)
    {
        // Cache le fantôme
        if (meshFantome != null)
        {
            meshFantome.SetActive(false);
        }
    }

    private void OnPieceRetiree(SelectExitEventArgs args)
    {
        // Réaffiche le fantôme si le joueur reprend la pièce
        if (meshFantome != null)
        {
            meshFantome.SetActive(true);
        }
    }
}