using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private float turnDurationInSeconds = 180;
    [SerializeField] private float jokeResponseDelayInSeconds = 2;
    [SerializeField] private int reputationLevel = 100;

    public static GameController Instance;

    public delegate void TimeCountdown();
    public static event TimeCountdown OnCountdown;

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
    }

    IEnumerator CountDownTurnDuration()
    {
        OnCountdown();

        while(turnDurationInSeconds > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            turnDurationInSeconds--;
            OnCountdown();
        }

        yield return null;
    }

    public void AddSecondsToTurn(float seconds)
    {
        turnDurationInSeconds += seconds;
    }

    public void SubtractSecondsToTurn(float seconds)
    {
        turnDurationInSeconds -= seconds;
    }

    public void AddReputationLevel(int amount)
    {
        reputationLevel += amount;
    }

        public void SubtractReputationLevel(int amount)
    {
        reputationLevel -= amount;
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