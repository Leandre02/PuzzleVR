using UnityEngine;

/// <summary>
/// Téléporte le bateau au loin à la fin de la partie.
/// </summary>
public class BateauQuiPart : MonoBehaviour
{

    private Vector3 positionDepart;

    void Start()
    {
        positionDepart = transform.position;
    }

    void OnEnable()
    {
        GestionnaireJeu.onFinDePartie += OnFinDePartie;
        GestionnaireJeu.onDebutPartie += OnDebutPartie;
    }

    void OnDisable()
    {
        GestionnaireJeu.onFinDePartie -= OnFinDePartie;
        GestionnaireJeu.onDebutPartie -= OnDebutPartie;
    }

    /// <summary>
    /// Téléporte le bateau à une position éloignée pour simuler son départ à la fin de la partie.
    /// </summary>
    /// <param name="toursCompletes"></param>
    private void OnFinDePartie(int toursCompletes)
    {
        transform.position = new Vector3(0, -100f, 0);
    }

    /// <summary>
    /// Place le bateau à sa position de départ au début de la partie, pour qu'il soit visible dans le menu et pendant la partie.
    /// </summary>
    private void OnDebutPartie()
    {
       
        transform.position = positionDepart; // revient au début
    }
}