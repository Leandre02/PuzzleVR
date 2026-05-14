using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

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

    private GameObject pieceActuelle;
    [SerializeField] private float delaiRespawn = 3f;

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
        pieceActuelle = null;
        StartCoroutine(SpawnAvecDelai()); // spawn la première pièce
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
        ArreterSpawn();
        pieceActuelle = null; // pièce placée, on passe à la suivante
        indexPiece++; 

        if (indexPiece >= prefabsPieces.Length) return;

        StartCoroutine(SpawnAvecDelai());
    }

    /// <summary>
    /// Coroutine pour spawner une pièce après un délai, en vérifiant si la pièce précédente est encore tenue par le joueur. Si oui, on attend avant de détruire et de spawn la suivante.
    /// Code genéré par Claude sonnet 4.6, Mars 2026
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnAvecDelai()
    {
        yield return new WaitForSeconds(delaiEntreSpawns);

        while (indexPiece < prefabsPieces.Length)
        {
            if (pieceActuelle != null)
            {
                // Vérifie si la pièce est en train d'être tenue
                var grab = pieceActuelle.GetComponent<XRGrabInteractable>();
                if (grab != null && grab.isSelected)
                {
                    // Joueur tient la pièce et attend sans détruire
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }
                Destroy(pieceActuelle);
            }

            int indexPoint = Random.Range(0, pointsDeSpawn.Length);
            pieceActuelle = Instantiate(prefabsPieces[indexPiece],
                pointsDeSpawn[indexPoint].position, Quaternion.identity);

            yield return new WaitForSeconds(delaiRespawn);
        }
    }
    // Fin code généré par Claude
}