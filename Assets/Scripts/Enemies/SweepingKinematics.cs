using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepingKinematics : Kinematics
{
    [SerializeField] float _sweepAngle = 45f;
    float _currentSweepAngle;
    bool _sweepingRight = true;
    protected override void HandleRotation()
    {
        if (_sweepingRight)
        {
            _currentSweepAngle += _speed * Time.deltaTime;
            if (_currentSweepAngle >= _sweepAngle)
            {
                _sweepingRight = false;
            }
        }
        else
        {
            _currentSweepAngle -= _speed * Time.deltaTime;
            if (_currentSweepAngle <= -_sweepAngle)
            {
                _sweepingRight = true;
            }
        }

        // Apply rotation based on the sweep angle
        transform.rotation = Quaternion.Euler(0, 0, _currentSweepAngle);

    }
}
