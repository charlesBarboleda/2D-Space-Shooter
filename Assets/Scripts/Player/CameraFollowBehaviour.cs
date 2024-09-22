using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraFollowBehaviour : MonoBehaviour
{
    public static CameraFollowBehaviour Instance { get; private set; }
    public CinemachineVirtualCamera playerCamera; // Main camera that follows the player
    public CinemachineVirtualCamera targetCamera; // Secondary camera for focusing on the target
    public float transitionDuration = 3f;      // Duration for the smooth transition

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
}
