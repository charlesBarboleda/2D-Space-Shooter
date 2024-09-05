using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    public Transform cameraTransform; // Reference to the camera transform
    public float shakeDuration = 0f; // Duration of the shake
    public float shakeMagnitude = 0.1f; // Magnitude of the shake
    public float dampingSpeed = 1.0f; // How fast the shake effect fades

    private Vector3 initialPosition;
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
            Debug.Log("Shaking... Duration remaining: " + shakeDurationRemaining);
            // Generate shake offset
            Vector3 shakeOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * shakeMagnitude;

            // Apply the shake to the camera
            cameraTransform.localPosition = initialPosition + shakeOffset;

            // Decrease shake duration
            shakeDurationRemaining -= Time.deltaTime * dampingSpeed;

            // If shake duration is over, reset position
            if (shakeDurationRemaining <= 0)
            {
                Debug.Log("Shake completed.");
                cameraTransform.localPosition = initialPosition;  // Reset camera to initial position
                shakeDurationRemaining = 0;  // Ensure no negative value
            }
        }
    }

    public void TriggerShake(float magnitude, float duration)
    {
        Debug.Log("Camera shake triggered!");
        shakeMagnitude = magnitude;
        shakeDurationRemaining = duration;
        initialPosition = cameraTransform.localPosition;
        Debug.Log("Shake duration: " + shakeDurationRemaining);
        Debug.Log("Shake magnitude: " + shakeMagnitude);
        Debug.Log("Initial Position: " + initialPosition);
    }

    public Vector3 GetShakeOffset()
    {
        if (shakeDurationRemaining > 0)
        {
            // Generate a random shake offset
            Vector3 shakeOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * shakeMagnitude;
            return shakeOffset;
        }

        return Vector3.zero; // No shake offset when shake is not active
    }
}
