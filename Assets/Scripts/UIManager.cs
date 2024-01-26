using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI jokeText;

    void OnEnable()
    {
        GameController.OnCountdown += ShowCountdownUI;
        PlayerController.OnJokeSelected += ShowJokeUI;
    }

    void OnDisable()
    {
        GameController.OnCountdown -= ShowCountdownUI;
        PlayerController.OnJokeSelected -= ShowJokeUI;
    }

    private void ShowCountdownUI(int timeRemaining)
    {
        timerText.text = timeRemaining.ToString();
    }

    private void ShowJokeUI(string joke, JokeQuality jokeQuality)
    {
        jokeText.text = joke;
    }
}
