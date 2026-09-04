using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private bool isDead;
    private GameUI gameUI;
    public bool IsDead => isDead;
    private Renderer[] playerRenderers;
    private Color[] originalColors;

    private void Awake()
    {
        currentHealth = maxHealth;
        isDead = false;
        gameUI = FindFirstObjectByType<GameUI>();
        playerRenderers = GetComponentsInChildren<Renderer>();

        originalColors = new Color[playerRenderers.Length];

        for (int i = 0; i < playerRenderers.Length; i++)
        {
            originalColors[i] = playerRenderers[i].material.color;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        if (playerRenderers != null)
        {
            for (int i = 0; i < playerRenderers.Length; i++)
            {
                playerRenderers[i].material.color = Color.red;
            }

            Invoke(nameof(ResetColor), 0.2f);
        }



        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    private void ResetColor()
    {
        if (playerRenderers != null)
        {
            for (int i = 0; i < playerRenderers.Length; i++)
            {
                playerRenderers[i].material.color = originalColors[i];
            }
        }
    }
    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        if (gameUI != null)
        {
            gameUI.ShowGameOver();
        }

        Debug.Log("Player Died!");
    }
}