using Unity.VisualScripting;
using UnityEngine;

public class JokeManager : MonoBehaviour
{
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
            OnTellJokeColliderVisualized();
        }
        else
        {
            OnTellJokeColliderUnvisualized();
        }

        if (Input.GetMouseButtonDown(1))
        {
            jokeSelected = null;
        }

        if(Input.GetMouseButtonDown(0))
        {
            if(jokeSelected != null)
            {
                crowdManager.CrowdResponse(jokeSelected.JokeQuality);
            }
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