using UnityEngine;

public class CollectableGainMove : MonoBehaviour, Collectable
{
    [SerializeField] private int movesToAdd = 1;

    public void CollectedBy(Player player)
    {
        player.AddMoves(movesToAdd);
    }
}