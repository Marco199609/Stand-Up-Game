using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
#region Events
    public delegate void JokeVisualized(JokePage jokePage);
    public static event JokeVisualized OnJokeVisualized;

    public delegate void JokeUnvisualized();
    public static event JokeUnvisualized OnJokeUnvisualized;

    public delegate void JokePageSelected(JokePage jokePage);
    public static event JokePageSelected OnJokePageSelected;

    private void OnEnable()
    {
        JokeManager.OnDeselectJokePage += ResetVirtualCam;
        CrowdManager.OnCrowdReactionTick += FreezePlayer;
        CrowdManager.OnCrowdReactionTick += ResetVirtualCam;
        GameController.OnGameOver += FreezePlayer;
    }

    private void OnDisable()
    {
        JokeManager.OnDeselectJokePage -= ResetVirtualCam;
        CrowdManager.OnCrowdReactionTick -= FreezePlayer;
        CrowdManager.OnCrowdReactionTick += ResetVirtualCam;
        GameController.OnGameOver -= FreezePlayer;
    }

#endregion

    [SerializeField] private CinemachineVirtualCamera virtualCam;
    public Collider currentVisualizedObject { get; private set; }
    public static PlayerController Instance;

    private Camera cam;
    private CinemachinePOV vcPOV;
    private RaycastHit hit;
    private float defaultPOVSpeed = 700f;
    private float defaultPOVDecelTime = 0.05f;

    private void Awake()
    {
        if(!Instance) Instance = this;
        else Destroy(this);

        cam = Camera.main;
        vcPOV = virtualCam.GetCinemachineComponent<CinemachinePOV>();
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
        OnJokeVisualized(jokePage);

        if (Input.GetMouseButtonDown(0))
        {
            FreezePlayer();

            OnJokePageSelected(jokePage);
        }
    }

    public void FreezePlayer(float delay = 1)
    {
        if(delay > 0)
        {
            SetVCam(0, 0);
        }
    }

    private void FreezePlayer(bool gameOver)
    {
        if(gameOver)
        {
            SetVCam(0, 0);
        }
    }

    public void ResetVirtualCam(float delay = 0)
    {
        if(delay <= 0)
        {
            SetVCam(defaultPOVSpeed, defaultPOVDecelTime);
        }
    }
    private void ResetVirtualCam()
    {
        SetVCam(defaultPOVSpeed, defaultPOVDecelTime);
    }

    private void SetVCam(float maxSpeed, float decelTime)
    {
        vcPOV.m_HorizontalAxis.m_MaxSpeed = maxSpeed;
        vcPOV.m_VerticalAxis.m_MaxSpeed = maxSpeed;

        vcPOV.m_HorizontalAxis.m_DecelTime = decelTime;
        vcPOV.m_VerticalAxis.m_DecelTime = decelTime;
    }
}