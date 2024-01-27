using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Collider currentVisualizedObject { get; private set; }

    private Camera cam;
    private RaycastHit hit;

    public delegate void JokeVisualized(string joke, JokeQuality jokeQuality);
    public static event JokeVisualized OnJokeVisualized;

    public delegate void JokeUnvisualized();
    public static event JokeUnvisualized OnJokeUnvisualized;

    public delegate void JokeSelected(JokeData joke);
    public static event JokeSelected OnJokeSelected;

    public static PlayerController Instance;

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

        cam = Camera.main;
    }

    void Update()
    {
        PlayerRaycast();
    }

    private void PlayerRaycast()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        ray.direction = cam.transform.forward;

        if(Physics.Raycast(ray, out hit))
        {
            currentVisualizedObject = hit.collider;
            if (hit.collider.TryGetComponent(out JokeData jokeData))
            {
                SelectJoke(jokeData);
            }
            else
            {
                OnJokeUnvisualized();
            }
        }
    }

    private void SelectJoke(JokeData jokeData)
    {
        OnJokeVisualized(jokeData.Joke, jokeData.JokeQuality);

        if (Input.GetMouseButtonDown(0))
        {
            OnJokeSelected(jokeData);
        }
    }
}