using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using SnowHorse.Utils;
using System.Net.Sockets;

public class UIManager : MonoBehaviour
{
    #region Events
    void OnEnable()
    {
        GameController.OnCountdown += ShowCountdownUI;
        GameController.OnModifyReputationLevel += ReputationLevelUI;
        GameController.OnGameOver += GameOverUI;
        GameController.OnAddBonusTime += AddBonusTimeUI;
        JokeManager.OnTellJokeColliderVisualized += ActivateTellJokeUI;
        JokeManager.OnTellJokeColliderUnvisualized += DeactivateTellJokeUI;
        JokeManager.OnJokeSelected += ShowJokeUI;
        JokeManager.OnJokeSelected += DeactivateDontHesitateUI;
        JokeManager.OnJokeTold += ClearJokeUI;
        JokeManager.OnJokeTold += ActivateDontHesitateUI;
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
        GameController.OnAddBonusTime -= AddBonusTimeUI;
        JokeManager.OnTellJokeColliderVisualized -= ActivateTellJokeUI;
        JokeManager.OnTellJokeColliderUnvisualized -= DeactivateTellJokeUI;
        JokeManager.OnJokePageSelected -= DeactivateCenterPoint;
        JokeManager.OnDeselectJokePage -= ActivateCenterPoint;
        JokeManager.OnJokeSelected -= ShowJokeUI;
        JokeManager.OnJokeSelected -= DeactivateDontHesitateUI;
        JokeManager.OnJokeTold -= ClearJokeUI;
        JokeManager.OnJokeTold -= ActivateDontHesitateUI;
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
    [SerializeField] private Image[] reputationCenterStars;
    [SerializeField] private GameObject uiContainer;
    [SerializeField] private GameObject pagePrompts;
    [SerializeField] private RectTransform addedBonusUI;
    [SerializeField] private TextMeshProUGUI dontHesitateText;
    [SerializeField] private AnimationCurve dontHesitateUICurve; 
    [SerializeField] private AnimationCurve bonusTextCurve;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private AudioSource reputationStarSource;
    [SerializeField] private AudioClip addReputationClip;
    [SerializeField] private AudioClip subtractReputationClip;

    private float dontHesitateDelayTime = 8;
    private float dontHesistateCurrentTime;
    private float dontHesitateUITime = 1f;

    private float reputationStarsUITime = 0.5f;
    
    private float dontHesitateLerpProgress;
    private float addedBonusUILerpProgress;
    private float reputationStarsLerpProgress;
    private int previousReputation;
    private bool isHesitating;


    private void Awake()
    {
        ClearJokeUI();
        stopWatchFill.fillAmount = 0;
        crowdStopwatchFill.fillAmount = 0;
        crowdStopwatchObject.SetActive(false);
        addedBonusUI.gameObject.SetActive(false);
        jokeText.gameObject.SetActive(true);
    }

    private void ShowCountdownUI(int timeRemaining)
    {
        stopWatchFill.fillAmount = 1 - ((float) timeRemaining / 120);
        timeText.text = timeRemaining.ToString();
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
        if(!GameController.Instance.IsBonusTime)
        {
            reputationLevelFill.fillAmount = (float) GameController.Instance.GetReputationLevel() / 100;

            int currentReputation = GameController.Instance.GetReputationLevel();

            if(currentReputation > 80)
            {
                reputationLevelImage.sprite = reputationStateSprites[3];
                reputationLevelFill.color = Color.green;
            }
            else if(currentReputation > 65)
            {
                reputationLevelImage.sprite = reputationStateSprites[2];
                reputationLevelFill.color = Color.green;
            }
            else if(currentReputation > 50)
            {
                reputationLevelImage.sprite = reputationStateSprites[1];
            }
            else
            {
                reputationLevelImage.sprite = reputationStateSprites[0];

                if(currentReputation > 35)
                {
                    reputationLevelFill.color = Color.yellow;
                }
                else
                {
                    reputationLevelFill.color = Color.red;
                }
            }

            ReputationStarAudio(currentReputation);
        }
    }

    private void ReputationStarAudio(int reputation)
    {
        if(previousReputation < reputation)
        {
            if(reputation == 55 || reputation == 70 || reputation == 85 || reputation >= 100)
            {
                if(reputation == 55)
                {
                    reputationStarSource.pitch = 1;
                    StartCoroutine(DisplayReputationCenterStars(reputationCenterStars[0]));
                }
                else if(reputation == 70)
                {
                    reputationStarSource.pitch = 1.05f;
                    StartCoroutine(DisplayReputationCenterStars(reputationCenterStars[1]));
                }
                else if(reputation == 85)
                {
                    reputationStarSource.pitch = 1.08f;
                    StartCoroutine(DisplayReputationCenterStars(reputationCenterStars[2]));
                }

                reputationStarSource.PlayOneShot(addReputationClip);
            }
        }
        else
        {
            if(reputation == 50 || reputation == 65 || reputation == 80)
            {
                reputationStarSource.pitch = 1;
                reputationStarSource.PlayOneShot(subtractReputationClip);
            }
        }

        previousReputation = reputation;
    }

    private IEnumerator DisplayReputationCenterStars(Image starImage)
    {
        Vector3 targetScale = Vector3.one * 2f;

        starImage.transform.localScale = Vector3.one;
        starImage.gameObject.SetActive(true);

        float percent = 0;

        while(percent < 1)
        {
            percent = Interpolation.Linear(reputationStarsUITime, ref reputationStarsLerpProgress);

            var animatedPercent = dontHesitateUICurve.Evaluate(percent);

            starImage.transform.localScale = Vector3.Slerp(Vector3.one, targetScale, percent);
            starImage.color = new Color(1, 1, 1, animatedPercent - 0.5f);

            dontHesistateCurrentTime -= 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        starImage.gameObject.SetActive(false);
        reputationStarsLerpProgress = 0;
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
    }

    private void AddBonusTimeUI()
    {
        addedBonusUI.gameObject.SetActive(true);

        reputationStarSource.pitch = 1.10f;
        reputationStarSource.PlayOneShot(addReputationClip);

        reputationLevelImage.sprite = reputationStateSprites[4];
        reputationLevelImage.color = Color.cyan;
        reputationLevelFill.color = Color.cyan;

        stopWatchFill.color = Color.green;
        DeactivateCenterPoint();
        StartCoroutine(AddBonusTimeManage());
    }

    private IEnumerator AddBonusTimeManage()
    {
        addedBonusUILerpProgress = 0;
        Vector3 targetScale = Vector3.one * 1.3f;
        var text = addedBonusUI.GetComponent<TextMeshProUGUI>();

        while(addedBonusUI.localScale != targetScale)
        {
            var percent = Interpolation.Smoother(1.5f, ref addedBonusUILerpProgress);
            addedBonusUI.localScale = Vector3.Slerp(Vector3.one, targetScale, percent);

            //bonusTextCurve.Evaluate(percent);
            text.color = new Color(1, 1 ,1, bonusTextCurve.Evaluate(percent));

            yield return new WaitForEndOfFrame();
        }
        addedBonusUI.gameObject.SetActive(false);
        addedBonusUI.localScale = Vector3.one;
        ActivateCenterPoint();
    }


    private void ActivateDontHesitateUI()
    {
        dontHesistateCurrentTime = dontHesitateDelayTime;
        dontHesitateLerpProgress = 0;
        isHesitating = true;
        StartCoroutine(DontHesitatUIManage());
    }

    private void DeactivateDontHesitateUI(string joke)
    {
        isHesitating = false;
        dontHesitateText.gameObject.SetActive(false);
    }

    private IEnumerator DontHesitatUIManage()
    {
        Vector3 targetScale = Vector3.one * 1.3f;

        while(isHesitating)
        {
            if(dontHesistateCurrentTime < 0)
            {
                dontHesitateText.gameObject.SetActive(true);

                var percent = Interpolation.Linear(dontHesitateUITime, ref dontHesitateLerpProgress);

                if(percent >= 1) dontHesitateLerpProgress = 0;

                var animatedPercent = dontHesitateUICurve.Evaluate(percent);

                dontHesitateText.transform.localScale = Vector3.Slerp(Vector3.one, targetScale, animatedPercent);
                dontHesitateText.color = new Color(1, 1, 1, animatedPercent);

            }

            dontHesistateCurrentTime -= 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}