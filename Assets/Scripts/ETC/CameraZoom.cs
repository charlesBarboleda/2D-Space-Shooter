using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomedInFOV = 10f; // Field of View for zoomed-in
    public float zoomedOutFOV = 20f; // Field of View for zoomed-out
    public float zoomSpeed = 5f; // Speed of zoom transition

    private bool isZoomedIn = false;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Replace KeyCode.Z with your desired button
        {
            isZoomedIn = !isZoomedIn; // Toggle zoom state
        }

        float targetFOV = isZoomedIn ? zoomedOutFOV : zoomedInFOV;
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetFOV, Time.deltaTime * zoomSpeed);
    }
}
