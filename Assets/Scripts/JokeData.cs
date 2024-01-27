using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class JokeData : MonoBehaviour
{
    public string joke;
    public JokeQuality jokequality = JokeQuality.GoodJoke;
}



public enum JokeQuality {
    GoodJoke,
    BadJoke
}

[Serializable]
public class JokeDataTest
{
    public JokeDataTest(){}
    public string joke { get; set; }
}

[Serializable]
public class JokeDataListObject 
{
    public JokeDataListObject(){}
    public List<JokeDataTest> jokedatas  { get; set; }
}
