using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI jokeText;

    void OnEnable()
    {
        GameController.OnCountdown += ShowCountdownUI;
        PlayerController.OnJokeVisualized += ShowJokeUI;
        PlayerController.OnJokeUnvisualized += CleanJokeUI;
    }

    void OnDisable()
    {
        GameController.OnCountdown -= ShowCountdownUI;
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
}
