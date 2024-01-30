using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SnowHorse.Utils;

public class CrowdManager : MonoBehaviour
{
    public delegate void CrowdReactionTick(float reactionDelay);
    public static event CrowdReactionTick OnCrowdReactionTick;


    [SerializeField] private AudioSource jokeAudioSource;
    [SerializeField] private AudioSource baDumTssAudioSource;
    [SerializeField] private AudioClip cheeringClip;
    [SerializeField] private AudioClip booingClip;
    [SerializeField] private List<Animator> characterAnimators;

    private float reactionDelay = 3;
    private float crowdDelayProgress;

    private void Start()
    {
        foreach(Animator animator in characterAnimators)
        {
            SetAnimationDelay(animator, "idle", "idle_2", 2);
        }
    }

    public void CrowdResponse(JokeQuality jokeQuality)
    {
        if(!JokeManager.Instance.isVisualizingJokeSheet)
        {
            StartCoroutine(CrowdReactionDelay(jokeQuality));
        }   
    }

    IEnumerator CrowdReactionDelay(JokeQuality jokeQuality)
    {
        crowdDelayProgress = 0;

        while (reactionDelay > 0)
        {
            float percent = Interpolation.Linear(reactionDelay, ref crowdDelayProgress);
            reactionDelay = Mathf.Lerp(3, 0, percent);

            OnCrowdReactionTick(reactionDelay);

            yield return new WaitForEndOfFrame();
        }

        reactionDelay = 3;

        switch (jokeQuality)
        {
            case JokeQuality.GoodJoke:
                GoodJokeResponse();
                break;
            case JokeQuality.BadJoke:
                BadJokeResponse();
                break;
        }
    }

    private void GoodJokeResponse()
    {
        foreach(Animator animator in characterAnimators)
        {
            SetAnimationDelay(animator, "sitting_clap", "standing_clap");
        }

        jokeAudioSource.PlayOneShot(cheeringClip, 0.6f);

        GameController.Instance.AddReputationLevel(5);
    }
    private void BadJokeResponse()
    {
        foreach(Animator animator in characterAnimators)
        {
            SetAnimationDelay(animator, "boo", "boo_2", 0.4f);
        }

        jokeAudioSource.PlayOneShot(booingClip, 0.2f);
        baDumTssAudioSource.Play();
        
        GameController.Instance.SubtractReputationLevel(10);
    }

    private void SetAnimationDelay(Animator animator, string trigger1, string trigger2, float maxDelayInSeconds = 1, float firstTriggerProbability = 0.5f)
    {
        int randDelay = (int) (maxDelayInSeconds * 100);
        float delay = (float) Random.Range(0, randDelay) / randDelay;

        string trigger = delay < (maxDelayInSeconds * firstTriggerProbability) ? trigger1 : trigger2;

        StartCoroutine(SetCrowdTriggers(animator, trigger, delay));
    }

    IEnumerator SetCrowdTriggers(Animator animator, string trigger, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        animator.SetTrigger(trigger);
    }
}