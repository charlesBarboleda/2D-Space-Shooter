using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBehaviour : MonoBehaviour
{

    [SerializeField] Camera _mainCamera;
    Vector3 _cameraOffset = new Vector3(0, 0, -30f);

    // Late Update is called after Update each frame
    void LateUpdate()
    {
        CameraFollow();
    }

    void CameraFollow()
    {
        Vector3 followPosition = transform.position + _cameraOffset;

        // If the camera shake is active, add the shake offset
        if (CameraShake.Instance != null && CameraShake.Instance.shakeDurationRemaining > 0)
        {
            followPosition += CameraShake.Instance.GetShakeOffset();
        }

        // Apply the final position to the camera
        _mainCamera.transform.position = followPosition;
    }
}
