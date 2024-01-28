using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class JokeData : MonoBehaviour
{
    public string Joke;
    public JokeQuality JokeQuality;

    public JokeData jokeData(Joke joke)
    {
        Joke = joke.joke;
        JokeQuality = joke.JokeQuality;
        return this;
    }
}

[Serializable]
public class JokeListObject
{
    [JsonProperty("joke_list")]
    public List<Joke> JokeList;

    public JokeListObject jokeArray(List<Joke> jokes)
    {
        JokeList = jokes;
        return this;
    }
}

[Serializable]
public class Joke
{
    [JsonProperty("joke")]
    public string joke;
    [JsonProperty("joke_quality")]
    public JokeQuality JokeQuality;

    public Joke jokeContainer(string joke, JokeQuality jokeQuality)
    {
        this.joke = joke;
        this.JokeQuality = jokeQuality;
        return this;
    }
}

public enum JokeQuality {
    GoodJoke,
    BadJoke
}