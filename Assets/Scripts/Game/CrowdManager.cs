using System.Collections;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    public delegate void CrowdReactionTick(int tick);
    public static event CrowdReactionTick OnCrowdReactionTick;


    [SerializeField] private AudioSource jokeAudioSource;
    [SerializeField] private AudioClip cheeringClip;
    [SerializeField] private AudioClip booingClip;

    private int reactionTick = 3;

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
}