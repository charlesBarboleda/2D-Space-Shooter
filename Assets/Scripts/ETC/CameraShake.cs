using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    public Transform cameraTransform; // Reference to the camera transform
    public float shakeDuration = 0f; // Duration of the shake
    public float shakeMagnitude = 0.1f; // Magnitude of the shake
    public float dampingSpeed = 1.0f; // How fast the shake effect fades

    private Vector3 initialPosition;
    private float shakeDurationRemaining;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Set to main camera if not assigned
        }
        initialPosition = cameraTransform.localPosition;
    }

    void Update()
    {
        if (shakeDurationRemaining > 0)
        {
            // Shake the camera
            cameraTransform.localPosition = transform.position + Random.insideUnitSphere * shakeMagnitude;

            // Decrease shake duration
            shakeDurationRemaining -= Time.deltaTime * dampingSpeed;
            Debug.Log(shakeDurationRemaining);

            // If shake duration is over, stop shaking
            if (shakeDurationRemaining <= 0)
            {
                shakeDurationRemaining = 0;
            }
        }
    }

    public void TriggerShake(float magnitude, float duration)
    {
        shakeMagnitude = magnitude;
        shakeDurationRemaining = duration;
    }
}
