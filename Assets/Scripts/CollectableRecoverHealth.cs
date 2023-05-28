using UnityEngine;

public class CollectableRecoverHealth : MonoBehaviour, Collectable
{
    [SerializeField] private int healthToAdd = 1;

    public void CollectedBy(Player player)
    {
        player.AddHealthPoints(healthToAdd);
    }
}