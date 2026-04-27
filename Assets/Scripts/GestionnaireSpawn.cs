using UnityEngine;
using System.Collections;

/// <summary>
/// Classe pour gérer le spawn des pièces du puzzle à des points aléatoires.
/// Chaque pièce est spawnée une seule fois par tour.
/// Inspirée des exercices de VR où des objets sont instanciés dynamiquement via des coroutines.
/// Références :
///  - Cégep de Victoriaville. Exercice 3 — Peinture VR. Environnements Immersifs, 2026.
///  - Cégep de Victoriaville. Travail pratique — Whack-a-Mole VR. Environnements Immersifs, 2026.
/// </summary>
public class GestionnaireSpawn : MonoBehaviour
{
    [Header("Prefabs des pièces")]
    [SerializeField] private GameObject[] prefabsPieces;

    [Header("Paramètres de spawn")]
    [SerializeField] private float delaiEntreSpawns = 0.3f;

    [Header("Points de spawn")]
    [SerializeField] private Transform[] pointsDeSpawn;

    private Coroutine spawnRoutine;

    /// <summary>
    /// Appelée par GestionnaireJeu quand un tour commence
    /// </summary>
    public void DemarrerSpawn()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }
        spawnRoutine = StartCoroutine(SpawnPieces());
    }

    /// <summary>
    /// Appelée par GestionnaireJeu quand la partie se termine
    /// </summary>
    public void ArreterSpawn()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    /// <summary>
    /// Spawne chaque pièce une seule fois à un point aléatoire
    /// </summary>
    IEnumerator SpawnPieces()
    {
        // Mélange les points de spawn pour que chaque pièce aille à un point différent
        Transform[] pointsMelanges = MelangerPoints();

        for (int i = 0; i < prefabsPieces.Length; i++)
        {
            // Un point de spawn différent pour chaque pièce
            Transform point = pointsMelanges[i % pointsMelanges.Length];

            Instantiate(prefabsPieces[i], point.position, Quaternion.identity);

            yield return new WaitForSeconds(delaiEntreSpawns);
        }
    }

    /// <summary>
    /// Mélange aléatoirement les points de spawn
    /// Code généré par Claude sonnet 4.6, Mars 2026
    /// </summary>
    Transform[] MelangerPoints()
    {
        Transform[] copie = (Transform[])pointsDeSpawn.Clone();
        for (int i = copie.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Transform temp = copie[i];
            copie[i] = copie[j];
            copie[j] = temp;
        }
        return copie;
    }
}