using UnityEngine;

public class WebGLAudioAdjuster : MonoBehaviour
{
    void Start()
    {
        // Adjust volume specifically for WebGL builds
#if UNITY_WEBGL
        AudioListener.volume = 0.3f; // Scale the volume down for WebGL (adjust to your needs)
#endif
    }
}