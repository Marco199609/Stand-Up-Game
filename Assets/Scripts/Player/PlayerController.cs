using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
#region Events
    public delegate void JokeVisualized(string joke, JokeQuality jokeQuality);
    public static event JokeVisualized OnJokeVisualized;

    public delegate void JokeUnvisualized();
    public static event JokeUnvisualized OnJokeUnvisualized;

    public delegate void JokePageSelected(JokePage jokePage);
    public static event JokePageSelected OnJokePageSelected;

    private void OnEnable()
    {
        JokeManager.OnDeselectJokePage += ResetVirtualCam;
    }

    private void OnDisable()
    {
        JokeManager.OnDeselectJokePage -= ResetVirtualCam;
    }

#endregion

    [SerializeField] private CinemachineVirtualCamera virtualCam;
    public Collider currentVisualizedObject { get; private set; }
    public static PlayerController Instance;

    private Camera cam;
    private RaycastHit hit;
    private float defaultPOVSpeed = 1.5f;
    private float defaultPOVDecelTime = 0.2f;

    private void Awake()
    {
        if(!Instance) Instance = this;
        else Destroy(this);

        cam = Camera.main;
    }

    void Update()
    {
        if(JokeManager.Instance.IsJokePageSelected == false)
        {
            PlayerRaycast();
        }
    }

    private void PlayerRaycast()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        ray.direction = cam.transform.forward;

        if(Physics.Raycast(ray, out hit))
        {
            currentVisualizedObject = hit.collider;
            if (hit.collider.TryGetComponent(out JokePage jokePage))
            {
                SelectJokePage(jokePage);
            }
            else
            {
                OnJokeUnvisualized();
            }
        }
    }

    private void SelectJokePage(JokePage jokePage)
    {
        OnJokeVisualized(jokePage.JokeData.Text, jokePage.JokeData.JokeQuality);

        if (Input.GetMouseButtonDown(0))
        {
            var pov = virtualCam.GetCinemachineComponent<CinemachinePOV>();
            pov.m_HorizontalAxis.m_MaxSpeed = 0;
            pov.m_VerticalAxis.m_MaxSpeed = 0;

            pov.m_HorizontalAxis.m_DecelTime = 0;
            pov.m_VerticalAxis.m_DecelTime = 0;

            OnJokePageSelected(jokePage);
        }
    }

    private void ResetVirtualCam()
    {
        var pov = virtualCam.GetCinemachineComponent<CinemachinePOV>();
        pov.m_HorizontalAxis.m_MaxSpeed = defaultPOVSpeed;
        pov.m_VerticalAxis.m_MaxSpeed = defaultPOVSpeed;

        pov.m_HorizontalAxis.m_DecelTime = defaultPOVDecelTime;
        pov.m_VerticalAxis.m_DecelTime = defaultPOVDecelTime;
    }
}