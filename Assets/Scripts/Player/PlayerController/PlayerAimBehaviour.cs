using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimBehaviour : MonoBehaviour
{
    Vector3 _mousePosition;
    Vector3 _direction;
    float _angle;
    // Update is called once per frame
    void Update()
    {
        Aim();
    }

    void Aim()
    {
        // Get the world position of the mouse cursor
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the GameObject to the mouse position
        _direction = _mousePosition - transform.position;

        // Calculate the angle in radians and convert to degrees
        _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the GameObject
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle + 270f));
    }
}
