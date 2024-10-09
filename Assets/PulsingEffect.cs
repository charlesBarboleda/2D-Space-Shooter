using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingEffect : MonoBehaviour
{
    public bool _shouldPulse = false;
    // Update is called once per frame
    void Update()
    {
        if (_shouldPulse) Pulse();
    }

    public void Pulse()
    {
        transform.localScale = new Vector3(1 + Mathf.PingPong(Time.time, 0.1f), 1 + Mathf.PingPong(Time.time, 0.1f), 1);
    }
}
