using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform[] backgroundLayers; // Assign different layers (e.g. stars, planets)
    public float[] parallaxScales; // Control how fast each layer moves relative to the camera
    public float smoothing = 1f; // How smooth the parallax effect is

    private Vector3 previousCameraPosition;

    void Start()
    {
        // Store the camera's initial position
        previousCameraPosition = Camera.main.transform.position;
    }

    void LateUpdate()
    {
        // Loop through each background layer and apply parallax
        for (int i = 0; i < backgroundLayers.Length; i++)
        {
            // Calculate the parallax movement for each layer
            float parallaxX = (previousCameraPosition.x - Camera.main.transform.position.x) * parallaxScales[i];
            float parallaxY = (previousCameraPosition.y - Camera.main.transform.position.y) * parallaxScales[i];

            // Update the layer's position
            Vector3 newPos = new Vector3(backgroundLayers[i].position.x + parallaxX, backgroundLayers[i].position.y + parallaxY, backgroundLayers[i].position.z);
            backgroundLayers[i].position = Vector3.Lerp(backgroundLayers[i].position, newPos, smoothing * Time.deltaTime);
        }

        // Update the camera position for the next frame
        previousCameraPosition = Camera.main.transform.position;
    }
}
