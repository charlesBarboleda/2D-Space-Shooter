using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtKinematics : Kinematics
{
    [SerializeField] Transform _target;
    [SerializeField] float _rotationSpeed = 50f;
    [SerializeField] float _offset;


    // Update is called once per frame
    protected override void Update()
    {
        transform.LookAt(_target.position);
        OrbitAround(_target.position);

        // Calculate direction to the target
        Vector3 direction = _target.position - transform.position;

        // Calculate the angle to rotate the ship so it faces the target on the Z-axis
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the ship towards the target without tilting
        transform.rotation = Quaternion.Euler(0, 0, angle + _offset);
    }

    void OrbitAround(Vector3 target)
    {
        // Orbit around the target on the z-axis
        transform.RotateAround(target, Vector3.forward, _rotationSpeed * Time.deltaTime);
    }
}
