using UnityEngine;

public class BobbleMotion : MonoBehaviour
{
    [SerializeField] float _amplitude = 0.5f;  // How far the object moves up and down
    [SerializeField] float _frequency = 1f;    // Speed of the bobbing motion
    Vector3 _startPos;

    void Start()
    {
        // Store the initial position of the object
        _startPos = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave for smooth up-and-down motion
        float newY = _startPos.y + Mathf.Sin(Time.time * _frequency) * _amplitude;

        // Update the object's position with the new Y position
        transform.position = new Vector3(_startPos.x, newY, _startPos.z);
    }
}
