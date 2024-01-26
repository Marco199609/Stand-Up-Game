using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera cam;
    private RaycastHit hit;

    public delegate void JokeVisualized(string joke, JokeQuality jokeQuality);
    public static event JokeVisualized OnJokeVisualized;

    public delegate void JokeUnvisualized();
    public static event JokeUnvisualized OnJokeUnvisualized;

    public delegate void JokeSelected(JokeData joke);
    public static event JokeSelected OnJokeSelected;

    private void Awake()
    {
        cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
