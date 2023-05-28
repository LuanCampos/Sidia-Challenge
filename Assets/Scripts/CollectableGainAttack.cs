using UnityEngine;

public class CollectableGainAttack : MonoBehaviour, Collectable
{
    [Header("Collectable Settings")]
    [Tooltip("The amount of attack points to add to the player.")]
    [SerializeField] private int attackToAdd = 1;

    public void CollectedBy(Player player)
    {
        player.AddAttackPoints(attackToAdd);
    }
}