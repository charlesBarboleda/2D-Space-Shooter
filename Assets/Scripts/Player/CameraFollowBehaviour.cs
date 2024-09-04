using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBehaviour : MonoBehaviour
{

    [SerializeField] Camera _mainCamera;

    // Update is called once per frame
    void Update()
    {
        CameraFollow();
    }

    void CameraFollow()
    {
        _mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -30f);
    }
}
