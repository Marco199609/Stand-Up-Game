using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    [SerializeField] private AudioSource jokeAudioSource;
    [SerializeField] private AudioClip cheeringClip;
    [SerializeField] private AudioClip booingClip;

    public void CrowdResponse(JokeQuality jokeQuality)
    {
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
    }
    private void BadJokeResponse()
    {
        if (jokeAudioSource.isPlaying)
        {
            jokeAudioSource.Stop();
        }
        jokeAudioSource.clip = booingClip;
        jokeAudioSource.Play();
    }
}
