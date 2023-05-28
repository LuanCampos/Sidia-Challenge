using UnityEngine;

public class CollectableGainAttack : MonoBehaviour, Collectable
{
    [SerializeField] private int attackToAdd = 1;

    public void CollectedBy(Player player)
    {
        player.AddAttackPoints(attackToAdd);
    }
}