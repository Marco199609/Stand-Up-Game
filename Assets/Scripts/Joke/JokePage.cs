using TMPro;
using UnityEngine;
using SnowHorse.Utils;
using System.Collections;

public class JokePage : MonoBehaviour
{
    [SerializeField] private Collider col;

    [field: SerializeField] public JokeData JokeData;
    [field: SerializeField] public TextMeshProUGUI pageText;

    private float lerpDelay = 0.3f;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private float lerpProgress;

    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void SetJoke(Joke joke)
    {
        pageText.text = joke.joke;
        JokeData.Text = joke.joke;
        JokeData.JokeQuality = joke.JokeQuality;
    }

    public void GoToViewingPosition(Transform pageHolder)
    {         
        StopAllCoroutines();   
        StartCoroutine(GoToViewPosition(pageHolder));
    }

    public void GoToInitialPosition()
    {
        StopAllCoroutines();
        StartCoroutine(GoToInitialPos());
    }

    public bool IsPageReady(Transform pageHolder)
    {
        return transform.position == pageHolder.position;
    }

    IEnumerator GoToViewPosition(Transform pageHolder)
    {
        lerpProgress = 0;

        col.enabled = false;
        
        Vector3 targetPosition = pageHolder.position;
        Quaternion targetRotation = pageHolder.rotation;

        while(transform.position != targetPosition)
        {
            float percent = Interpolation.Sinerp(lerpDelay, ref lerpProgress);

            transform.position = Vector3.Lerp(initialPosition, targetPosition, percent);
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, percent);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator GoToInitialPos()
    {
        if(lerpProgress > 0)
        {
            lerpProgress = 0;
            Vector3 startPosition = transform.position;
            Quaternion startRotation = transform.rotation;

            Vector3 targetPosition = initialPosition;
            Quaternion targetRotation = initialRotation;

            while(transform.position != initialPosition)
            {
                float percent = Interpolation.Sinerp(lerpDelay, ref lerpProgress);

                transform.position = Vector3.Lerp(startPosition, targetPosition, percent);
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, percent);
                yield return new WaitForEndOfFrame();
            }

            col.enabled = true;
        }
    }
}
