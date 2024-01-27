using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI jokeText;
    [SerializeField] private GameObject tellJokeToCrowdUI;
    [SerializeField] private GameObject uiCenterPoint;
    [SerializeField] private Image reputationLevelImage;

    void OnEnable()
    {
        GameController.OnCountdown += ShowCountdownUI;
        GameController.OnModifyReputationLevel += ReputationLevelUI;
        JokeManager.OnTellJokeColliderVisualized += ActivateTellJokeUI;
        JokeManager.OnTellJokeColliderUnvisualized += DeactivateTellJokeUI;
        PlayerController.OnJokeVisualized += ShowJokeUI;
        PlayerController.OnJokeUnvisualized += CleanJokeUI;
    }

    void OnDisable()
    {
        GameController.OnCountdown -= ShowCountdownUI;
        GameController.OnModifyReputationLevel -= ReputationLevelUI;
        JokeManager.OnTellJokeColliderVisualized -= ActivateTellJokeUI;
        JokeManager.OnTellJokeColliderUnvisualized -= DeactivateTellJokeUI;
        PlayerController.OnJokeVisualized -= ShowJokeUI;
        PlayerController.OnJokeUnvisualized -= CleanJokeUI;
    }

    private void ShowCountdownUI(int timeRemaining)
    {
        timerText.text = timeRemaining.ToString();
    }

    private void ShowJokeUI(string joke, JokeQuality jokeQuality)
    {
        jokeText.text = joke;
    }

    private void CleanJokeUI()
    {
        jokeText.text = string.Empty;
    }

    private void ActivateTellJokeUI()
    {
        tellJokeToCrowdUI.SetActive(true);
        uiCenterPoint.SetActive(false);
    }

    private void DeactivateTellJokeUI()
    {
        tellJokeToCrowdUI.SetActive(false);
        uiCenterPoint.SetActive(true);
    }

    private void ReputationLevelUI()
    {
        reputationLevelImage.fillAmount = (float) GameController.Instance.GetReputationLevel() / 100;
    }
}