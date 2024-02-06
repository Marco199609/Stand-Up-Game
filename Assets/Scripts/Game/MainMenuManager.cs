using System.Collections;
using SnowHorse.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Slider slider;

    private float sensitivity = 1;
    private float blackScreenLerpProgress;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DontDestroyOnLoad(this);
    }

    public void StartGame()
    {
        StartCoroutine(GameStart());
    }

    private IEnumerator GameStart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        sensitivity = slider.value;

        blackScreen.SetActive(true);

        Vector3 initialScale = blackScreen.transform.localScale;
        Vector3 targetScale = Vector3.one * 100;

        while(blackScreen.transform.localScale != targetScale)
        {
            var percent = Interpolation.Linear(1.5f, ref blackScreenLerpProgress);

            blackScreen.transform.localScale = Vector3.Lerp(initialScale, targetScale, percent);

            yield return null;
        }

        audioSource.Stop();
        SceneManager.LoadScene("Level1");

        while(GameController.Instance == null)
        {
            yield return null;
        }

        GameController.Instance.VCamSensitivity = sensitivity;

        Destroy(gameObject);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        //TODO
    }
}
