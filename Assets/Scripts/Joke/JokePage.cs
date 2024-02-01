using TMPro;
using UnityEngine;
using SnowHorse.Utils;
using System.Collections;

public class JokePage : MonoBehaviour
{
    [SerializeField] private Collider col;
    [SerializeField] private GameObject pageModel;

    [field: SerializeField] public JokeData JokeData;
    [field: SerializeField] public TextMeshProUGUI pageText;
    

    private float lerpDelay = 0.3f;
    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Quaternion initialRotation;
    private float lerpProgress;
    private float visualizationLerpProgress;
    private bool canMoveModel;

    private Coroutine goToViewingPosition;
    private Coroutine goToInitialPosition;

    private Coroutine resetModelPosition;
    private Coroutine modelGoToVisualizationPosition;

    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    public void SetJoke(Joke joke)
    {
        pageText.text = joke.joke;
        JokeData.Text = joke.joke;
        JokeData.JokeQuality = joke.JokeQuality;
    }

    public void GoToViewingPosition(Transform pageHolder)
    {         
        if(goToInitialPosition != null) StopCoroutine(goToInitialPosition);   
        goToViewingPosition = StartCoroutine(GoToViewJokePos(pageHolder));

        if(modelGoToVisualizationPosition != null) StopCoroutine(modelGoToVisualizationPosition);
        resetModelPosition = StartCoroutine(ModelGoToVisualizedPos(Vector3.zero, 0.2f, false));
    }

    public void GoToInitialPosition()
    {
        if(goToViewingPosition != null) StopCoroutine(goToViewingPosition);
        goToInitialPosition = StartCoroutine(GoToInitialPos());
    }
    public void ModelGoToVisualizationPosition()
    {
        if(!canMoveModel)
        {
            if(resetModelPosition != null) StopCoroutine(resetModelPosition);
            modelGoToVisualizationPosition = StartCoroutine(ModelGoToVisualizedPos(new Vector3(0, 0.15f, 0), 0.4f, true));
        }
    }

    public void ResetModelPosition()
    {
        if(canMoveModel)
        {
            if(modelGoToVisualizationPosition != null) StopCoroutine(modelGoToVisualizationPosition);
            resetModelPosition = StartCoroutine(ModelGoToVisualizedPos(Vector3.zero, 0.4f, false));
        }
    }
    public bool IsPageReady(Transform pageHolder)
    {
        return transform.position == pageHolder.position;
    }

    IEnumerator ModelGoToVisualizedPos(Vector3 targetPos, float delay, bool canMoveModel)
    {
        visualizationLerpProgress = 0;
        Vector3 initialPos = pageModel.transform.localPosition;

        while (pageModel.transform.localPosition != targetPos)
        {
            float percent = Interpolation.Sinerp(delay, ref visualizationLerpProgress);

            pageModel.transform.localPosition = Vector3.Lerp(initialPos, targetPos, percent);
            yield return new WaitForEndOfFrame();
        }

        this.canMoveModel = canMoveModel;
    }

    IEnumerator GoToViewJokePos(Transform pageHolder)
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
            transform.localScale = Vector3.Lerp(initialScale, initialScale * 0.6f, percent);

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
            Vector3 startScale = transform.localScale;

            Vector3 targetPosition = initialPosition;
            Quaternion targetRotation = initialRotation;
            Vector3 targetScale = initialScale;

            while(transform.position != initialPosition)
            {
                float percent = Interpolation.Sinerp(lerpDelay, ref lerpProgress);

                transform.position = Vector3.Lerp(startPosition, targetPosition, percent);
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, percent);
                transform.localScale = Vector3.Lerp(startScale, targetScale, percent);
                yield return new WaitForEndOfFrame();
            }

            col.enabled = true;
        }
    }
}
