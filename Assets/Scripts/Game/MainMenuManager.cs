using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using SnowHorse.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private AudioSource audioSource;

    private float blackScreenLerpProgress;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        StartCoroutine(GameStart());
    }

    private IEnumerator GameStart()
    {
        blackScreen.SetActive(true);

        Vector3 initialScale = blackScreen.transform.localScale;
        Vector3 targetScale = Vector3.one * 100;

        while(blackScreen.transform.localScale != Vector3.one * 100)
        {
            var percent = Interpolation.Linear(1.5f, ref blackScreenLerpProgress);

            blackScreen.transform.localScale = Vector3.Lerp(initialScale, targetScale, percent);

            yield return null;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        audioSource.Stop();
        SceneManager.LoadScene("Level1");
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
