using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBehaviour : MonoBehaviour
{

    [SerializeField] Camera _mainCamera;
    [SerializeField] Transform _playerTransform;

    [SerializeField] float _damping = 0.1f; // Adjust this value to control the damping effect

    Vector3 _offset; // Optional: if you want to keep an offset from the player

    void Start()
    {
        if (_playerTransform != null)
        {
            _offset = _mainCamera.transform.position - _playerTransform.position;
        }
    }

    void FixedUpdate()
    {
        if (_playerTransform != null)
        {
            // Desired position with optional offset
            Vector3 desiredPosition = _playerTransform.position + _offset;
            desiredPosition.z = -10; // Ensure the camera stays at the same z position

            // Smoothly interpolate camera position towards the desired position
            _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, desiredPosition, _damping);
        }
    }
}
