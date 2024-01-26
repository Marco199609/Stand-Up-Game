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

    private void ShowCountdownUI()
    {
        timerText.text = GameController.Instance.GetTurnTimeLeft().ToString();
    }
}
