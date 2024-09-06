using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour
{
    Transform _target;
    [SerializeField] float _speed = 5f;
    [SerializeField] float _curveAngle = 10f;

    int _currentWaypointIndex = 0;

    void Update()
    {
        Vector3 targetPosition = _target.position;
        Vector3 rotatedDirection = Quaternion.Euler(0, _curveAngle, 0) * transform.forward;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + rotatedDirection * _speed * Time.deltaTime, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {

            gameObject.SetActive(false);
        }
    }
    public float Speed { get => _speed; set => _speed = value; }
    public float CurveAngle { get => _curveAngle; set => _curveAngle = value; }
    public Transform Target { get => _target; set => _target = value; }

}
