using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private int turnDurationInSeconds = 90;
    [SerializeField] private float jokeResponseDelayInSeconds = 2;
    [SerializeField] private int reputationLevel = 50;

    public static GameController Instance;

    public delegate void TimeCountdown(int timeRemaining);
    public static event TimeCountdown OnCountdown;

    public delegate void ModifyReputationLevel();
    public static event ModifyReputationLevel OnModifyReputationLevel;

    public delegate void GameOver();
    public static event GameOver OnGameOver;

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(CountDownTurnDuration());

        OnModifyReputationLevel();
    }

    private void Update()
    {
        if(reputationLevel <= 0)
        {
            OnGameOver();
            Time.timeScale = 0;
        }
    }

    IEnumerator CountDownTurnDuration()
    {
        OnCountdown(turnDurationInSeconds);

        while(turnDurationInSeconds > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            turnDurationInSeconds--;
            OnCountdown(turnDurationInSeconds);
        }

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