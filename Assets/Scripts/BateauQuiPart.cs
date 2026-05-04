using UnityEngine;

/// <summary>
/// Une classe d'animation pour faire avancer un bateau vers l'avant à une vitesse spécifiée.
/// </summary>
public class BateauQuiPart : MonoBehaviour
{
    public float vitesse = 0.5f; // La vitesse du bateau
    public float vitesseFuite = 5f; // vitesse quand le temps est écoulé

    private float vitesseActuelle;
    private bool partieTerminee = false;

    void Start()
    {
        vitesseActuelle = vitesse; // Initialise la vitesse actuelle à la vitesse de base
    }

    void Update()
    {
        // Fait avancer le bateau vers l'avant à la vitesse spécifiée
        transform.Translate(Vector3.forward * vitesseActuelle * Time.deltaTime);

        if (partieTerminee && transform.position.magnitude > 200f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// S'abonne à l'Evenement
    /// </summary>
    void OnEnable()
    {
        GestionnaireJeu.onFinDePartie += OnFinDePartie;
    }

    /// <summary>
    /// Se desabonne à l'Evenement pour éviter les fuites mémoire
    /// </summary>
    void OnDisable()
    {
        GestionnaireJeu.onFinDePartie -= OnFinDePartie;
    }

    /// <summary>
    /// Une methode pour gérer la fin de partie, appelée par le GestionnaireJeu via un événement.
    /// </summary>
    /// <param name="toursCompletes">Le nombre de tours complétés avant la fin de la partie</param>
    private void OnFinDePartie(int toursCompletes)
    {
        partieTerminee = true;
        vitesseActuelle = vitesseFuite;
    }
}
