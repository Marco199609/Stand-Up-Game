using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    #region Events
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
        JokeManager.OnDeselectJokePage += DectivatePagePrompts;
        JokeManager.OnJokePageSelected += ActivatePagePrompts;
        CrowdManager.OnCrowdReactionTick += CrowdDelayUI;
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
        JokeManager.OnDeselectJokePage -= DectivatePagePrompts;
        JokeManager.OnJokePageSelected -= ActivatePagePrompts;
        CrowdManager.OnCrowdReactionTick -= CrowdDelayUI;
    }

    #endregion

    [SerializeField] private Image stopWatchFill;
    [SerializeField] private Image crowdStopwatchFill;
    [SerializeField] private GameObject crowdStopwatchObject;
    [SerializeField] private TextMeshProUGUI jokeText;
    [SerializeField] private GameObject tellJokeToCrowdUI;
    [SerializeField] private GameObject uiCenterPoint;
    [SerializeField] private Image reputationLevelImage;
    [SerializeField] private Sprite[] reputationStateSprites;
    [SerializeField] private Image reputationLevelFill;
    [SerializeField] private GameObject uiContainer;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject pagePrompts;

    private void Awake()
    {
        ClearJokeUI();
        stopWatchFill.fillAmount = 0;
        crowdStopwatchFill.fillAmount = 0;
        crowdStopwatchObject.SetActive(false);
        jokeText.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void ShowCountdownUI(int timeRemaining)
    {
        stopWatchFill.fillAmount = 1 - ((float) timeRemaining / 90);
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
        if(!crowdStopwatchObject.activeInHierarchy)
        {
            uiCenterPoint.SetActive(true);
        }
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
        reputationLevelFill.fillAmount = (float) GameController.Instance.GetReputationLevel() / 100;

        int currentReputation = GameController.Instance.GetReputationLevel();

        if(currentReputation > 80)
        {
            reputationLevelImage.sprite = reputationStateSprites[3];
        }
        else if(currentReputation > 65)
        {
            reputationLevelImage.sprite = reputationStateSprites[2];
        }
        else if(currentReputation > 50)
        {
            reputationLevelImage.sprite = reputationStateSprites[1];
        }
        else
        {
            reputationLevelImage.sprite = reputationStateSprites[0];
        }
    }

    private void CrowdDelayUI(float fillAmount)
    {
        crowdStopwatchFill.fillAmount = 1 - (fillAmount / 3);

        if(fillAmount > 0)
        {
            DeactivateCenterPoint();
            crowdStopwatchObject.SetActive(true);
            
        }
        else
        {
            ActivateCenterPoint();
            crowdStopwatchObject.SetActive(false);
        }
    }

    private void ActivatePagePrompts(JokePage page)
    {
        pagePrompts.SetActive(true);
    }

    private void DectivatePagePrompts()
    {
        pagePrompts.SetActive(false);
    }

    private void GameOverUI(bool gameOver)
    {
        uiContainer.SetActive(false);
        gameOverUI.SetActive(true);
    }
}