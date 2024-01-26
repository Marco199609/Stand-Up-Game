using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private float turnDurationInSeconds = 180;
    [SerializeField] private int reputationLevel = 100;

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

    public int GetReputationLevel()
    {
        return reputationLevel;
    }
}
