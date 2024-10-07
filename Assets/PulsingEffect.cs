using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingEffect : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // Make the object pulse
        transform.localScale = new Vector3(1 + Mathf.PingPong(Time.time, 0.1f), 1 + Mathf.PingPong(Time.time, 0.1f), 1);
    }
}
