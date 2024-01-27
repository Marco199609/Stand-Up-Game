using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;
using System.Linq;

public class JokeManager : MonoBehaviour
{
    [SerializeField] private List<JokeDataTest> jokeList;
    [SerializeField] private GameObject[] jokePages;
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
        var json = Resources.Load("Jokes");
        var output = json.ToString();
        var listObject = JsonUtility.FromJson<JokeDataListObject>(output);

        jokeList = listObject.jokedatas;
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
                    crowdManager.CrowdResponse(jokeSelected.jokequality);
                    jokeSelected = null;
                }
            }
        }
        else
        {
            OnTellJokeColliderUnvisualized();
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