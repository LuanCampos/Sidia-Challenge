using UnityEngine;

public class CollectableGainMove : MonoBehaviour, Collectable
{
    [Header("Collectable Settings")]
    [Tooltip("The amount of attack points to add to the player.")]
    [SerializeField] private int movesToAdd = 1;

    public void CollectedBy(Player player)
    {
        player.AddMoves(movesToAdd);
    }
}