using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [Tooltip("The amount of health points the player has.")]
    [SerializeField] private int healthPoints = 10;
    [Tooltip("The amount of attack points the player has.")]
    [SerializeField] private int attackPoints = 1;
    [Tooltip("The particle effect to spawn when the player dies.")]
    [SerializeField] private GameObject deathParticlePrefab;
    [Tooltip("The lifetime of the particle effect.")]
    [SerializeField] private float particleLifetime = 0.5f;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void AddMoves(int moves)
    {
        gameManager.AddMovesToCurrentPlayer(moves);
        gameManager.PlaySound("GainMove");
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
        gameManager.PlaySound("GainHealth");
    }

    public void AddAttackPoints(int attack)
    {
        attackPoints += attack;
        gameManager.PlaySound("GainAttack");
    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;

        if (healthPoints <= 0)
            Die();
    }

    private void Die()
    {
        gameManager.PlaySound("PlayerDeath");
        GameObject particleEffect = Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        Destroy(particleEffect, particleLifetime);
        gameManager.PlayerDied(gameObject);
    }
}