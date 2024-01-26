using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokeData : MonoBehaviour
{
    [field: SerializeField] public string Joke {get; private set; }
    [field: SerializeField] public JokeQuality JokeQuality {get; private set; }
}

public enum JokeQuality {
    GoodJoke,
    BadJoke
}
