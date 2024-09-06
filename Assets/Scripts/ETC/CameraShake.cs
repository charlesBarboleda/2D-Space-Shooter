using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    public float shakeDuration = 0f; // Duration of the shake
    public float shakeMagnitude = 0.1f; // Magnitude of the shake
    public float dampingSpeed = 1.0f; // How fast the shake effect fades

    private CinemachineBasicMultiChannelPerlin _perlinNoise;
    public float shakeDurationRemaining;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);  // Destroy this duplicate instance
        }
    }

    void Start()
    {
        if (_virtualCamera == null)
        {
            _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>(); // Find the virtual camera if not assigned
        }

        _perlinNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (shakeDurationRemaining > 0)
        {

            // Adjust the noise amplitude for shake effect
            _perlinNoise.m_AmplitudeGain = shakeMagnitude;

            // Decrease shake duration
            shakeDurationRemaining -= Time.deltaTime * dampingSpeed;

            // If shake duration is over, reset the noise
            if (shakeDurationRemaining <= 0)
            {
                _perlinNoise.m_AmplitudeGain = 0;  // Reset shake
                shakeDurationRemaining = 0;  // Ensure no negative value
            }
        }
    }

    public void TriggerShake(float magnitude, float duration)
    {

        shakeMagnitude = magnitude;
        shakeDurationRemaining = duration;

        // Start shaking the camera with the new magnitude and duration

        _perlinNoise.m_AmplitudeGain = shakeMagnitude;

    }

    public Vector3 GetShakeOffset()
    {
        // Cinemachine handles shake internally via noise, no need to calculate manually
        return Vector3.zero; // No shake offset needed here
    }
}
