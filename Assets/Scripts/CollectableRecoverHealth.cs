using UnityEngine;

public class CollectableRecoverHealth : MonoBehaviour, Collectable
{
    [Header("Collectable Settings")]
    [Tooltip("The amount of attack points to add to the player.")]
    [SerializeField] private int healthToAdd = 1;

    public void CollectedBy(Player player)
    {
        player.AddHealthPoints(healthToAdd);
    }
}