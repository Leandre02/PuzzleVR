using UnityEngine;

/// <summary>
/// Gère la musique de menu et la musique de jeu.
/// </summary>
public class GestionnaireAudio : MonoBehaviour
{
    [Header("Musiques")]
    [SerializeField] private AudioClip SonAmbiance;
    [SerializeField] private AudioClip musiqueJeu;
    [SerializeField] private AudioSource sourceMusique;

    void Start()
    {
        JouerMusique(SonAmbiance); // démarre avec la musique du menu
    }

    /// <summary>
    /// S'abonne à l'evenement de fin de partie 
    /// </summary>
    void OnEnable()
    {
        GestionnaireJeu.onFinDePartie += OnFinDePartie;
    }

    /// <summary>
    /// Se desabonne à l'evenement de fin de partie pour éviter les fuites mémoire
    /// </summary>
    void OnDisable()
    {
        GestionnaireJeu.onFinDePartie -= OnFinDePartie;
    }

    /// <summary>
    /// Une methode pour gérer la fin de partie, appelée par le GestionnaireJeu via un événement.
    /// </summary>
    /// <param name="toursCompletes">Le nombre de tours complétés avant la fin de la partie.</param>
    private void OnFinDePartie(int toursCompletes)
    {
        JouerMusique(SonAmbiance); // revient à la musique du menu à la fin de la partie
    }

    /// <summary>
    /// Une methode pour jouer la musique de jeu, appelée par le GestionnaireJeu au début de la partie.
    /// </summary>
    public void JouerMusiqueJeu()
    {
        JouerMusique(musiqueJeu);
    }

    /// <summary>
    /// Lit le clip audio spécifié en tant que musique de fond, remplaçant toute musique en cours.
    /// </summary>
    /// <remarks>Si le paramètre <paramref name="clip"/> est null, aucune action n'est effectuée et la musique
    /// en cours n'est pas modifiée.</remarks>
    /// <param name="clip">Le clip audio à lire. Ne peut pas être null.</param>
    private void JouerMusique(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        sourceMusique.clip = clip;
        sourceMusique.Play();
    }
}