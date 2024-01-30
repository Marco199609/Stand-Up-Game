using System.Collections;
using SnowHorse.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region Events
    public static GameController Instance;
    public delegate void TimeCountdown(int timeRemaining);
    public static event TimeCountdown OnCountdown;

    public delegate void ModifyReputationLevel();
    public static event ModifyReputationLevel OnModifyReputationLevel;
    public delegate void GameOver(bool gameOver);
    public static event GameOver OnGameOver;
    #endregion
    
    #region GameScripts
    [SerializeField] private PlayerController playerController;
    [SerializeField] private JokeManager jokeManager;
    [SerializeField] private UIManager uiManager;
    #endregion

    [SerializeField] private float specialUISetDelay = 3;
    [SerializeField] private int turnDurationInSeconds = 90;
    [SerializeField] private float jokeResponseDelayInSeconds = 2;
    [SerializeField] private int reputationLevel = 50;
    [SerializeField] private int bonusDuration = 15;
    [SerializeField] private AudioSource tickSource;
    [SerializeField] private Image specialUIBackground;
    [SerializeField] private TextMeshProUGUI specialUIText;
    [SerializeField] private GameObject gameOverButtons;
    [SerializeField, TextArea] private string startGameText;
    [SerializeField, TextArea] private string goodEndingText;
    [SerializeField, TextArea] private string normalEndingText;
    [SerializeField, TextArea] private string badEndingText;


    private bool inGame;
    private float transparencyLerpProgress;

    private Coroutine countDownTurnDuration;

    private void Awake()
    {
        Application.targetFrameRate = 0;
        QualitySettings.vSyncCount = 0;

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        specialUIText.text = startGameText;
    }

    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if(!inGame)
        {
            StartCoroutine(InitializeGame());
            inGame = true;
        }
    }

    private IEnumerator InitializeGame()
    {
        float transparency = 1;
        transparencyLerpProgress = 0;
        while(transparency > 0)
        {
            var percent = Interpolation.Linear(specialUISetDelay, ref transparencyLerpProgress, true);

            transparency = Mathf.Lerp(1, 0, percent);
            specialUIBackground.color = new Color(0, 0, 0, transparency);
            yield return new WaitForEndOfFrame();
        }

        specialUIBackground.gameObject.SetActive(false);
        playerController.enabled = true;
        jokeManager.enabled = true;
        uiManager.enabled = true;

        playerController.ResetVirtualCam();

        countDownTurnDuration = StartCoroutine(CountDownTurnDuration());
        OnModifyReputationLevel();
    }

    private IEnumerator GameEnding()
    {
        specialUIBackground.gameObject.SetActive(true);

        if(reputationLevel <= 0)
        {
            specialUIText.text = badEndingText;
        }
        else if (turnDurationInSeconds <= 0)
        {
            if(reputationLevel > 80)
            {
                specialUIText.text = goodEndingText;
            }
            else
            {
                specialUIText.text = normalEndingText;
            }
        }
        else
        {
            specialUIText.text = normalEndingText;
            Debug.Log("Check conditions in game controller ending!");
        }

        float transparency = 0;
        transparencyLerpProgress = 0;

        while(transparency < 1)
        {
            var percent = Interpolation.Linear(specialUISetDelay, ref transparencyLerpProgress);

            transparency = Mathf.Lerp(0, 1, percent);
            specialUIBackground.color = new Color(0, 0, 0, transparency);
            yield return new WaitForEndOfFrame();
        }

        specialUIText.gameObject.SetActive(true);
        gameOverButtons.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Intermediate");
    }

    public void Quit()
    {
        Application.Quit();
    }



    private void Update()
    {
        if(reputationLevel <= 0 || turnDurationInSeconds <= 0)
        {
            if(inGame)
            {
                OnGameOver(true);
                StopCoroutine(countDownTurnDuration);
                StartCoroutine(GameEnding());
                inGame = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    IEnumerator CountDownTurnDuration()
    {
        OnCountdown(turnDurationInSeconds);

        while(turnDurationInSeconds > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            tickSource.Play();
            turnDurationInSeconds--;
            OnCountdown(turnDurationInSeconds);
        }

        OnGameOver(true);

        yield return null;
    }

    public void AddSecondsToTurn(int seconds)
    {
        turnDurationInSeconds += seconds;
    }

    public void SubtractSecondsToTurn(int seconds)
    {
        turnDurationInSeconds -= seconds;
    }

    public void AddReputationLevel(int amount)
    {
        reputationLevel += amount;
        reputationLevel = Mathf.Clamp(reputationLevel, 0, 100);

        if(reputationLevel >= 100)
        {
            turnDurationInSeconds += bonusDuration;
            reputationLevel = 50;
        }

        OnModifyReputationLevel();
    }

        public void SubtractReputationLevel(int amount)
    {
        reputationLevel -= amount;
        reputationLevel = Mathf.Clamp(reputationLevel, 0, 100);
        OnModifyReputationLevel();
    }

    public float GetTurnTimeLeft()
    {
        return turnDurationInSeconds;
    }

    public float GetJokeResponseDelay()
    {
        return jokeResponseDelayInSeconds;
    }

    public int GetReputationLevel()
    {
        return reputationLevel;
    }
}