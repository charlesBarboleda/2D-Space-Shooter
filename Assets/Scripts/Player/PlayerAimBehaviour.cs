using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerAimBehaviour : MonoBehaviour
{
    Camera _mainCamera;
    Vector3 _mousePosition;
    Vector3 _direction;
    float _angle;

    void Start()
    {
        _mainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        Aim();
    }

    void Aim()
    {
        // Get the world position of the mouse cursor through the main camera
        _mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0;  // Ensure the z-axis doesn't cause issues in 2D

        // Calculate the direction from the GameObject to the mouse position
        _direction = _mousePosition - transform.position;

        // Calculate the angle in radians and convert to degrees
        _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the GameObject
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle + 90f));
    }
}
