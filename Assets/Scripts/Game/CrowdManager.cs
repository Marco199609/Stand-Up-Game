using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    public delegate void CrowdReactionTick(int tick);
    public static event CrowdReactionTick OnCrowdReactionTick;


    [SerializeField] private AudioSource jokeAudioSource;
    [SerializeField] private AudioClip cheeringClip;
    [SerializeField] private AudioClip booingClip;
    [SerializeField] private List<Animator> characterAnimators;

    private int reactionTick = 3;

    private void Start()
    {
        foreach(Animator animator in characterAnimators)
        {
            SetInitialAnimationDelay(animator);
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
        while (reactionTick > 0)
        {
            OnCrowdReactionTick(reactionTick);
            yield return new WaitForSecondsRealtime(1);
            reactionTick--;
        }

        OnCrowdReactionTick(reactionTick);

        reactionTick = 3;

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
            SelectGoodResponseAnimation(animator);
        }

        if(jokeAudioSource.isPlaying)
        {
            jokeAudioSource.Stop();
        }

        jokeAudioSource.clip = cheeringClip;
        jokeAudioSource.Play();

        GameController.Instance.AddReputationLevel(5);
    }
    private void BadJokeResponse()
    {
        if (jokeAudioSource.isPlaying)
        {
            jokeAudioSource.Stop();
        }
        jokeAudioSource.clip = booingClip;
        jokeAudioSource.Play();
        
        GameController.Instance.SubtractReputationLevel(10);
    }

    private void SetInitialAnimationDelay(Animator animator)
    {
        int randDelay = 200;
        float delay = (float) Random.Range(0, randDelay) / randDelay;

        string trigger = delay < 1 ? "idle" : "idle_2";

        StartCoroutine(SetCrowdTriggers(animator, trigger, delay));
    }

    private void SelectGoodResponseAnimation(Animator animator)
    {
        int randDelay = 100;
        float delay = (float) Random.Range(0, randDelay) / randDelay;
        string trigger = delay > 0.5f ? "sitting_clap" : "standing_clap";

        StartCoroutine(SetCrowdTriggers(animator, trigger, delay));
    }

    IEnumerator SetCrowdTriggers(Animator animator, string trigger, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        animator.SetTrigger(trigger);
    }
}