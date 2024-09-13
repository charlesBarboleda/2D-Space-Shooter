using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    [SerializeField] private Camera _mainCamera; // Reference to the main camera
    public float shakeDuration = 0f; // Duration of the shake
    public float shakeMagnitude = 0.1f; // Magnitude of the shake
    public float dampingSpeed = 1.0f; // How fast the shake effect fades

    private Vector3 _originalCameraPosition;
    private float _shakeDurationRemaining;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Destroy this duplicate instance
        }
    }

    void Start()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main; // Find the main camera if not assigned
        }
    }

    void Update()
    {
        if (_shakeDurationRemaining > 0)
        {
            // Calculate shake offset
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            shakeOffset.z = 0; // Ensure shake is only on the X and Y axes

            // Apply the shake offset to the camera
            _mainCamera.transform.localPosition = _originalCameraPosition + shakeOffset;

            // Decrease shake duration
            _shakeDurationRemaining -= Time.deltaTime * dampingSpeed;

            // If shake duration is over, reset the camera position
            if (_shakeDurationRemaining <= 0)
            {
                _mainCamera.transform.localPosition = _originalCameraPosition;
                _shakeDurationRemaining = 0; // Ensure no negative value
            }
        }
    }

    public void TriggerShake(float magnitude, float duration)
    {
        shakeMagnitude = magnitude;
        _shakeDurationRemaining = duration;

        // Save the original camera position
        _originalCameraPosition = _mainCamera.transform.localPosition;
    }

    public Vector3 GetShakeOffset()
    {
        // Return the current shake offset
        return _mainCamera.transform.localPosition - _originalCameraPosition;
    }
}
