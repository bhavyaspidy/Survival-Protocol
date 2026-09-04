using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private Image xpBarFill;
    [SerializeField] private TMP_Text levelUpText;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private TMP_Text waveText;


    public void UpdateLevel(int level)
    {
        levelText.text = "LEVEL " + level;
    }

    public void ShowLevelUp()
    {
        levelUpText.gameObject.SetActive(true);

        CancelInvoke(nameof(HideLevelUp));
        Invoke(nameof(HideLevelUp), 2f);
    }
    public void ShowGameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.SetActive(true);
    }


    private void HideLevelUp()
    {
        levelUpText.gameObject.SetActive(false);
    }

    public void UpdateXP(int currentXP, int requiredXP)
    {
        xpText.text = "XP: " + currentXP + " / " + requiredXP;

        float xpPercentage =
            (float)currentXP / requiredXP;

        xpBarFill.fillAmount = xpPercentage;
    }
    public void UpdateWave(int wave)
    {
        waveText.text = "WAVE " + wave;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}