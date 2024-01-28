using UnityEngine;
using Newtonsoft.Json;

public class JokeManager : MonoBehaviour
{
    #region Events
    public delegate void TellJokeColliderVisualized();
    public static event TellJokeColliderVisualized OnTellJokeColliderVisualized;
    public delegate void TellJokeColliderUnvsualized();
    public static event TellJokeColliderUnvsualized OnTellJokeColliderUnvisualized;
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
        PlayerController.OnJokeVisualized += JokePageVisualized;
        PlayerController.OnJokeUnvisualized += JokeSheetUnvisualized;
        PlayerController.OnJokePageSelected += SelectJokePage;  
    }

    private void OnDisable()
    {
        PlayerController.OnJokeVisualized -= JokePageVisualized;
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

                if(Input.GetMouseButtonDown(0))
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

    private void ShuffleJokes()
    {
        foreach(JokePage jokePage in jokePages)
        {
            var i = Random.Range(0, jokeList.JokeList.Count - 1);
            jokePage.SetJoke(jokeList.JokeList[i]);
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

    private void JokePageVisualized(string joke, JokeQuality jokeQuality)
    {
        isVisualizingJokeSheet = true;
    }

    private void JokeSheetUnvisualized()
    {
        isVisualizingJokeSheet = false;
    }
}