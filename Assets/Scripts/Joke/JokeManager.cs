using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;

public class JokeManager : MonoBehaviour
{
    #region Events
    public delegate void TellJokeColliderVisualized();
    public static event TellJokeColliderVisualized OnTellJokeColliderVisualized;
    public delegate void TellJokeColliderUnvisualized();
    public static event TellJokeColliderUnvisualized OnTellJokeColliderUnvisualized;
    public delegate void JokeTold();
    public static event JokeTold OnJokeTold;
    public delegate void JokeSelected(string joke);
    public static event JokeSelected OnJokeSelected;
    public delegate void DeselectPage();
    public static event DeselectPage OnDeselectJokePage;
    public delegate void PageSelected(JokePage jokePage);
    public static event PageSelected OnJokePageSelected;

    private void OnEnable()
    {
        PlayerController.OnJokeVisualized += JokePageVisualization;
        PlayerController.OnJokeUnvisualized += JokeSheetUnvisualized;
        PlayerController.OnJokePageSelected += SelectJokePage;  
    }

    private void OnDisable()
    {
        PlayerController.OnJokeVisualized -= JokePageVisualization;
        PlayerController.OnJokeUnvisualized -= JokeSheetUnvisualized;
        PlayerController.OnJokePageSelected -= SelectJokePage;
    }

    #endregion

    [SerializeField] private TextAsset json;
    [SerializeField] private JokeListObject jokeList;
    [SerializeField] private JokePage[] jokePages;
    [SerializeField] private Transform pageHolder;
    [SerializeField] private Collider TellJokeToCrowdCollider;
    [SerializeField] private CrowdManager crowdManager;

    public bool IsJokePageSelected { get; private set; }

    public bool isVisualizingJokeSheet { get; private set; }

    public static JokeManager Instance;

    private List<Joke> goodJokeList;
    private List<Joke> badJokeList;

    private JokePage visualizedPage;
    private JokePage selectedJokePage;
    private JokeData currentJoke;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        jokeList = JsonConvert.DeserializeObject<JokeListObject>(json.text);

        ClassifyJokes();
        ShuffleJokes();
    }

    private void Update()
    {
        if(PlayerController.Instance.currentVisualizedObject == TellJokeToCrowdCollider)
        {
            if(currentJoke == null)
            {
                OnTellJokeColliderUnvisualized();
            }
            else
            {
                OnTellJokeColliderVisualized();

                if (Input.GetMouseButtonDown(0))
                {
                    TellJoke();
                }
            }
        }
        else if(!IsJokePageSelected)
        {
            OnTellJokeColliderUnvisualized();
        }

        if(IsJokePageSelected)
        {
            if(Input.GetMouseButtonDown(1))
            {
                DeselectJokePage();

                if(currentJoke != null)
                {
                    OnJokeSelected(currentJoke.Text);
                }
            }
            else if(Input.GetMouseButtonDown(0))
            {
                if(selectedJokePage.IsPageReady(pageHolder))
                {
                    SelectJoke();
                    DeselectJokePage();
                }
            }
        }
    }

    private void ClassifyJokes()
    {
        goodJokeList = new List<Joke>();
        badJokeList = new List<Joke>();

        foreach (Joke joke in jokeList.JokeList)
        {
            if(joke.JokeQuality == JokeQuality.GoodJoke) goodJokeList.Add(joke);
            else badJokeList.Add(joke);
        }
    }

    private void ShuffleJokes()
    {
        if(goodJokeList.Count < 2 || badJokeList.Count < 2)
        {
            ClassifyJokes();
        }

        var currentJokeList = new List<Joke>();

        for(int i = 0; i < 4; i++)
        {
            if(i == 0 || i == 2)
            {
                var j = Random.Range(0, goodJokeList.Count);

                currentJokeList.Add(goodJokeList[j]);
                goodJokeList.Remove(goodJokeList[j]);
            }
            else if(i == 1 || i == 3)
            {
                var j = Random.Range(0, badJokeList.Count);
                currentJokeList.Add(badJokeList[j]);
                badJokeList.Remove(badJokeList[j]);
            }
            else
            {
                Debug.Log("Creating current list incorrectly! Check joke manager.");
            }
        }

        foreach(JokePage jokePage in jokePages)
        {
            var i = Random.Range(0, currentJokeList.Count);
            jokePage.SetJoke(currentJokeList[i]);

            currentJokeList.Remove(currentJokeList[i]);
        }
    }
    private void SelectJoke()
    {
        currentJoke = selectedJokePage.JokeData;
        OnJokeSelected(currentJoke.Text);
    }
    private void SelectJokePage(JokePage jokePageSelected)
    {
        selectedJokePage = jokePageSelected;
        selectedJokePage.GoToViewingPosition(pageHolder);
        IsJokePageSelected = true;
        OnJokePageSelected(jokePageSelected);
    }

    private void DeselectJokePage()
    {
        selectedJokePage.GoToInitialPosition();
        IsJokePageSelected = false;
        OnDeselectJokePage();
    }


    private void TellJoke()
    {
        crowdManager.CrowdResponse(currentJoke.JokeQuality);
        ShuffleJokes();
        currentJoke = null;
        OnJokeTold();
    }

    private void JokeSheetUnvisualized()
    {
        JokePageVisualization(null);
    }

    private void JokePageVisualization(JokePage jokePage)
    {
        if(visualizedPage != null && visualizedPage != jokePage)
        {
            while(visualizedPage.CanMoveModel)
            {
                visualizedPage.ResetModelPosition();
            }
        }

        if(jokePage != null)
        {
            isVisualizingJokeSheet = true;
            visualizedPage = jokePage;
            visualizedPage.ModelGoToVisualizationPosition();
        }
        else
        {
            isVisualizingJokeSheet = false;
        }
    }
}