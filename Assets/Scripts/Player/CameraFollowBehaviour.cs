using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBehaviour : MonoBehaviour
{

    [SerializeField] Camera _mainCamera;
    [SerializeField] Transform _playerTransform;

    void FixedUpdate()
    {
        if (_playerTransform != null)
        {
            Vector3 playerPosition = _playerTransform.position;
            playerPosition.z = -10;
            _mainCamera.transform.position = playerPosition;
        }
    }
}
