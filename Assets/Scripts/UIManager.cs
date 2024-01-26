using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    void OnEnable()
    {
        GameController.OnCountdown += ShowCountdownUI;
    }

    void OnDisable()
    {
        GameController.OnCountdown -= ShowCountdownUI;
    }

    private void ShowCountdownUI(int timeRemaining)
    {
        timerText.text = timeRemaining.ToString();
    }
}
