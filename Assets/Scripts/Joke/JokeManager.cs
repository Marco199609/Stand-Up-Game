using UnityEngine;
using Newtonsoft.Json;

public class JokeManager : MonoBehaviour
{
    [SerializeField] private TextAsset json;
    [SerializeField] private JokeListObject jokeList;
    [SerializeField] private JokePage[] jokePages;
    [SerializeField] private JokeData jokeSelected;

    [SerializeField] private Collider TellJokeToCrowdCollider;
    [SerializeField] private CrowdManager crowdManager;

    public bool isVisualizingJokeSheet { get; private set; }

    public delegate void TellJokeColliderVisualized();
    public static event TellJokeColliderVisualized OnTellJokeColliderVisualized;
    public delegate void TellJokeColliderUnvsualized();
    public static event TellJokeColliderVisualized OnTellJokeColliderUnvisualized;

    public static JokeManager Instance;

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


    private void OnEnable()
    {
        PlayerController.OnJokeVisualized += JokeSheetVisualized;
        PlayerController.OnJokeUnvisualized += JokeSheetUnvisualized;
        PlayerController.OnJokeSelected += SetSelectedJoke;   
    }

    private void OnDisable()
    {
        PlayerController.OnJokeVisualized -= JokeSheetVisualized;
        PlayerController.OnJokeUnvisualized -= JokeSheetUnvisualized;
        PlayerController.OnJokeSelected -= SetSelectedJoke;
    }

    private void Update()
    {
        if(PlayerController.Instance.currentVisualizedObject == TellJokeToCrowdCollider)
        {
            if(jokeSelected == null)
            {
                OnTellJokeColliderUnvisualized();
            }
            else
            {
                OnTellJokeColliderVisualized();

                if(Input.GetMouseButtonDown(0))
                {
                    crowdManager.CrowdResponse(jokeSelected.JokeQuality);
                    ShuffleJokes();
                    jokeSelected = null;
                }
            }
        }
        else
        {
            OnTellJokeColliderUnvisualized();
        }
    }

    private void ShuffleJokes()
    {
        foreach(JokePage jokePage in jokePages)
        {
            var i = Random.Range(0, jokeList.JokeList.Count - 1);

            var jokeContainer = jokeList.JokeList[i];
            
            jokePage.pageText.text = jokeContainer.joke;
            jokePage.jokeData.Joke = jokeContainer.joke;
            jokePage.jokeData.JokeQuality = jokeContainer.JokeQuality;

            //jokeList.JokeList.Remove(jokeContainer);
        }
    }

    private void SetSelectedJoke(JokeData jokeSelected)
    {
        this.jokeSelected = jokeSelected;
    }

    private void JokeSheetVisualized(string joke, JokeQuality jokeQuality)
    {
        isVisualizingJokeSheet = true;
    }

    private void JokeSheetUnvisualized()
    {
        isVisualizingJokeSheet = false;
    }
}