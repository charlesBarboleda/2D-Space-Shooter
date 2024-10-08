using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraFollowBehaviour : MonoBehaviour
{
    public static CameraFollowBehaviour Instance { get; private set; }
    public CinemachineVirtualCamera playerCamera; // Main camera that follows the player
    public CinemachineVirtualCamera targetCamera; // Secondary camera for focusing on the target
    public float transitionDuration = 3f;      // Duration for the smooth transition

    private CinemachineBasicMultiChannelPerlin playerNoise; // Reference to the noise component for player camera
    private CinemachineBasicMultiChannelPerlin targetNoise; // Reference to the noise component for target camera
    private bool isShaking = false; // Flag to ensure shake doesn't overlap

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Make sure the player camera is active at the start
        ActivatePlayerCamera();

        // Get the noise components for both cameras
        playerNoise = playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        targetNoise = targetCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Call this method to pan to the target camera
    public void ActivateTargetCamera(Transform target)
    {
        // Enable the target camera and give it higher priority
        targetCamera.Follow = target;
        targetCamera.Priority = 11;
        playerCamera.Priority = 10;
    }

    // Call this method to pan back to the player camera
    public void ActivatePlayerCamera()
    {
        // Enable the player camera and give it higher priority
        playerCamera.Priority = 11;
        targetCamera.Priority = 10;
    }

    // Optionally, call this method to smoothly pan back after a delay (useful for cutscenes)
    public IEnumerator PanToTargetAndBack(Transform target, float waitTime)
    {
        // Pan to the target camera
        ActivateTargetCamera(target);

        // Wait for some time at the target
        yield return new WaitForSeconds(waitTime);

        // Pan back to the player camera
        ActivatePlayerCamera();
    }

    // Function to start camera shake on the player camera
    public void ShakePlayerCamera(float amplitude, float frequency, float duration)
    {
        if (!isShaking) // Ensure that shake isn't already in progress
        {
            StartCoroutine(ShakeCameraCoroutine(playerNoise, amplitude, frequency, duration));
        }
    }

    public void IncreaseTargetOrthographicSize(float targetSize, float duration)
    {
        StartCoroutine(ChangeOrthographicSize(targetSize, duration));
    }

    public void IncreasePlayerOrthographicSize(float targetSize, float duration)
    {
        StartCoroutine(ChangePlayerOrthographicSize(targetSize, duration));
    }

    public IEnumerator ChangePlayerOrthographicSize(float targetSize, float duration)
    {
        float startSize = playerCamera.m_Lens.OrthographicSize;
        playerCamera.m_Lens.OrthographicSize = targetSize;
        yield return new WaitForSeconds(duration);
        float t = 0f;

        while (t < 2f)
        {
            t += Time.deltaTime;
            playerCamera.m_Lens.OrthographicSize = Mathf.Lerp(targetSize, startSize, t);
            yield return null;
        }
        playerCamera.m_Lens.OrthographicSize = startSize;



    }

    public IEnumerator ChangeOrthographicSize(float targetSize, float duration)
    {
        float startSize = targetCamera.m_Lens.OrthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            targetCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);
            Debug.Log("Changing orthographic size to " + targetSize);
            Debug.Log("Current orthographic size: " + targetCamera.m_Lens.OrthographicSize);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }

    // Function to start camera shake on the target camera
    public void ShakeTargetCamera(float amplitude, float frequency, float duration)
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCameraCoroutine(targetNoise, amplitude, frequency, duration));
        }
    }

    // Coroutine that handles the custom camera shake effect
    private IEnumerator ShakeCameraCoroutine(CinemachineBasicMultiChannelPerlin noise, float amplitude, float frequency, float duration)
    {
        isShaking = true;

        // Set the noise parameters
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Reset the noise parameters to zero
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;

        isShaking = false;
    }
}
