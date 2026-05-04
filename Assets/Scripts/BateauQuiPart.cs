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

    private void OnFinDePartie(int toursCompletes)
    {
        transform.position = new Vector3(0, -100f, 0);
    }

    private void OnDebutPartie()
    {
       
        transform.position = positionDepart; // revient au début
    }
}