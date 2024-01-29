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
            SetAnimationDelay(animator, "sitting_clap", "standing_clap");
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
        foreach(Animator animator in characterAnimators)
        {
            SetAnimationDelay(animator, "boo", "boo_2", 0.4f);
        }

        if (jokeAudioSource.isPlaying)
        {
            jokeAudioSource.Stop();
        }
        jokeAudioSource.clip = booingClip;
        jokeAudioSource.Play();
        
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