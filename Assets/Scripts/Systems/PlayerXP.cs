using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    [SerializeField] private int startingXP = 0;
    [SerializeField] private int firstLevelXP = 100;
    [SerializeField] private int xpIncreasePerLevel = 50;

    private int totalXP;
    private int currentLevel = 1;

    private GameUI gameUI;

    private void Awake()
    {
        totalXP = startingXP;
        gameUI = FindFirstObjectByType<GameUI>();
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddXP(int amount)
    {
        totalXP += amount;

        CheckLevelUp();

        UpdateUI();

        Debug.Log(
            "Total XP: " + totalXP +
            " | Level: " + currentLevel
        );
    }

    private void CheckLevelUp()
    {
        while (totalXP >= GetXPRequiredForNextLevel())
        {
            currentLevel++;
            if (gameUI != null)
            {
                gameUI.ShowLevelUp();
            }

            PlayerShooting playerShooting =
                GetComponent<PlayerShooting>();

            if (playerShooting != null)
            {
                playerShooting.IncreaseDamage();
            }

            Debug.Log(
                "LEVEL UP! New Level: " +
                currentLevel
            );
            PlayerMovement playerMovement =
    GetComponent<PlayerMovement>();

            if (playerMovement != null)
            {
                playerMovement.IncreaseMoveSpeed();
            }
        }
    }

    private int GetXPRequiredForNextLevel()
    {
        int requiredXP = firstLevelXP;

        for (int level = 2; level <= currentLevel; level++)
        {
            requiredXP +=
                firstLevelXP +
                (level - 1) * xpIncreasePerLevel;
        }

        return requiredXP;
    }

    private int GetXPRequiredForCurrentLevel()
    {
        if (currentLevel == 1)
            return 0;

        int requiredXP = firstLevelXP;

        for (int level = 2; level < currentLevel; level++)
        {
            requiredXP +=
                firstLevelXP +
                (level - 1) * xpIncreasePerLevel;
        }

        return requiredXP;
    }

    private void UpdateUI()
    {
        if (gameUI == null)
            return;

        int currentLevelXP =
            totalXP -
            GetXPRequiredForCurrentLevel();

        int nextLevelXP =
            GetXPRequiredForNextLevel() -
            GetXPRequiredForCurrentLevel();

        gameUI.UpdateLevel(currentLevel);

        gameUI.UpdateXP(
            currentLevelXP,
            nextLevelXP
        );
    }
}