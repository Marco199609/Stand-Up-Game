using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Events
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI jokeText;
    [SerializeField] private GameObject tellJokeToCrowdUI;
    [SerializeField] private GameObject uiCenterPoint;
    [SerializeField] private Image reputationLevelImage;
    [SerializeField] private GameObject uiContainer;
    [SerializeField] private GameObject gameOverUI;

    void OnEnable()
    {
        GameController.OnCountdown += ShowCountdownUI;
        GameController.OnModifyReputationLevel += ReputationLevelUI;
        GameController.OnGameOver += GameOverUI;
        JokeManager.OnTellJokeColliderVisualized += ActivateTellJokeUI;
        JokeManager.OnTellJokeColliderUnvisualized += DeactivateTellJokeUI;
        JokeManager.OnJokeSelected += ShowJokeUI;
        JokeManager.OnJokeTold += ClearJokeUI;
        JokeManager.OnDeselectJokePage += ActivateCenterPoint;
        JokeManager.OnJokePageSelected += DeactivateCenterPoint;
        JokeManager.OnTellJokeColliderVisualized += DeactivateCenterPoint;
        JokeManager.OnTellJokeColliderUnvisualized += ActivateCenterPoint;
        JokeManager.OnJokePageSelected += ClearJokeUI;
    }

    void OnDisable()
    {
        GameController.OnCountdown -= ShowCountdownUI;
        GameController.OnModifyReputationLevel -= ReputationLevelUI;
        GameController.OnGameOver -= GameOverUI;
        JokeManager.OnTellJokeColliderVisualized -= ActivateTellJokeUI;
        JokeManager.OnTellJokeColliderUnvisualized -= DeactivateTellJokeUI;
        JokeManager.OnJokePageSelected -= DeactivateCenterPoint;
        JokeManager.OnDeselectJokePage -= ActivateCenterPoint;
        JokeManager.OnJokeSelected -= ShowJokeUI;
        JokeManager.OnJokeTold -= ClearJokeUI;
        JokeManager.OnTellJokeColliderVisualized -= DeactivateCenterPoint;
        JokeManager.OnTellJokeColliderUnvisualized -= ActivateCenterPoint;
        JokeManager.OnJokePageSelected -= ClearJokeUI;
    }

    #endregion

    private void Awake()
    {
        ClearJokeUI();
    }

    private void ShowCountdownUI(int timeRemaining)
    {
        timerText.text = timeRemaining.ToString();
    }

    private void ShowJokeUI(string joke)
    {
        jokeText.text = joke;
    }

    private void ClearJokeUI()
    {
        jokeText.text = string.Empty;
    }

    private void ClearJokeUI(JokePage page)
    {
        jokeText.text = string.Empty;
    }

    private void ActivateTellJokeUI()
    {
        tellJokeToCrowdUI.SetActive(true);
    }

    private void DeactivateTellJokeUI()
    {
        tellJokeToCrowdUI.SetActive(false);
    }

    private void ActivateCenterPoint()
    {
        uiCenterPoint.SetActive(true);
    }

    private void DeactivateCenterPoint(JokePage jokePage)
    {
        uiCenterPoint.SetActive(false);
    }

        private void DeactivateCenterPoint()
    {
        uiCenterPoint.SetActive(false);
    }

    private void ReputationLevelUI()
    {
        reputationLevelImage.fillAmount = (float) GameController.Instance.GetReputationLevel() / 100;
    }

    private void GameOverUI()
    {
        uiContainer.SetActive(false);
        gameOverUI.SetActive(true);
    }
}