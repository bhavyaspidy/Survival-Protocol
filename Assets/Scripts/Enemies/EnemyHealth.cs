using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private int xpReward = 25;

    private float currentHealth;
    private bool isDead;
    private Renderer[] enemyRenderers;
    private Color[] originalColors;

    private void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;
        enemyRenderers = GetComponentsInChildren<Renderer>();

        originalColors = new Color[enemyRenderers.Length];

        for (int i = 0; i < enemyRenderers.Length; i++)
        {
            originalColors[i] = enemyRenderers[i].material.color;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        if (enemyRenderers != null)
        {
            for (int i = 0; i < enemyRenderers.Length; i++)
            {
                enemyRenderers[i].material.color = Color.red;
            }

            Invoke(nameof(ResetColor), 0.2f);
        }

        currentHealth = Mathf.Clamp(
            currentHealth,
            0f,
            maxHealth
        );

        Debug.Log("Enemy Health: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    private void ResetColor()
    {
        if (enemyRenderers != null)
        {
            for (int i = 0; i < enemyRenderers.Length; i++)
            {
                enemyRenderers[i].material.color = originalColors[i];
            }
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        PlayerXP playerXP = FindFirstObjectByType<PlayerXP>();

        if (playerXP != null)
        {
            playerXP.AddXP(xpReward);
        }

        Debug.Log("Enemy Died!");

        EnemySpawner spawner =
            FindFirstObjectByType<EnemySpawner>();

        if (spawner != null)
        {
            spawner.EnemyDied();
        }

        Destroy(gameObject);
    }
}