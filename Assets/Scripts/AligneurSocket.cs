using UnityEngine;

/// <summary>
/// Aligne automatiquement la position du socket sur un objet cible.
/// </summary>
public class AligneurSocket : MonoBehaviour
{
    [SerializeField] private Transform cible;

    void Start()
    {
        if (cible != null)
        {
            transform.position = cible.position;
            transform.rotation = cible.rotation;
        }
    }
}