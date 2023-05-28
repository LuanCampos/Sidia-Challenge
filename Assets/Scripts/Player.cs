using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int healthPoints = 10;
    [SerializeField] private int attackPoints = 1;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void AddMoves(int moves)
    {
        gameManager.AddMovesToCurrentPlayer(moves);
    }

    public int GetAttackPoints()
    {
        return attackPoints;
    }

    public int GetHealthPoints()
    {
        return healthPoints;
    }

    public void AddHealthPoints(int health)
    {
        healthPoints += health;
    }

    public void AddAttackPoints(int attack)
    {
        attackPoints += attack;
    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;

        if (healthPoints <= 0)
            Die();
    }

    private void Die()
    {
        gameManager.PlayerDied(gameObject);
    }
}