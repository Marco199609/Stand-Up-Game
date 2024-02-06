using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public float VCamSensitivity = 1;

    public static GameSettings Instance;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(this); }

        DontDestroyOnLoad(Instance);
    }
}
