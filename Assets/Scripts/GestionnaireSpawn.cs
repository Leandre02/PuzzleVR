using UnityEngine;
using System.Collections;

/// <summary>
/// Spawne les pièces une par une. La pièce suivante apparaît
/// seulement quand la précédente est correctement placée.
/// </summary>
public class GestionnaireSpawn : MonoBehaviour
{
    [Header("Prefabs des pièces")]
    [SerializeField] private GameObject[] prefabsPieces;

    [Header("Paramètres de spawn")]
    [SerializeField] private float delaiEntreSpawns = 0.3f;

    [Header("Points de spawn")]
    [SerializeField] private Transform[] pointsDeSpawn;

    private int indexPiece = 0;

    void OnEnable()
    {
        GestionnairePuzzle.OnPieceDeposee += SpawnProchainepiece;
    }

    void OnDisable()
    {
        GestionnairePuzzle.OnPieceDeposee -= SpawnProchainepiece;
    }

    /// <summary>
    /// Appelée par GestionnaireJeu au début de chaque tour
    /// </summary>
    public void DemarrerSpawn()
    {
        indexPiece = 0;
        SpawnProchainepiece(); // spawn la première pièce
    }

    /// <summary>
    /// Appelée par GestionnaireJeu quand la partie se termine
    /// </summary>
    public void ArreterSpawn()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Spawne la prochaine pièce dans la liste
    /// </summary>
    void SpawnProchainepiece()
    {
        if (indexPiece >= prefabsPieces.Length) return;

        StartCoroutine(SpawnAvecDelai());
    }

    IEnumerator SpawnAvecDelai()
    {
        yield return new WaitForSeconds(delaiEntreSpawns);

        // Point aléatoire parmi les points dispo
        int indexPoint = Random.Range(0, pointsDeSpawn.Length);
        Instantiate(prefabsPieces[indexPiece], pointsDeSpawn[indexPoint].position, Quaternion.identity);

        indexPiece++;
    }
}