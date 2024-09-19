using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingKinematics : Kinematics
{
    [SerializeField] protected float _rotationSpeed = 100f;

    protected override void HandleRotation()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }
}
