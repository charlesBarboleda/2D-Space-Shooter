using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public float teleportDistance = 1f;
    public float teleportCount;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (teleportCount > 0)
            {
                ShipTeleport();
                teleportCount--;
            }
            else if (teleportCount <= 0)
            {
                Debug.Log("Out of Teleports");
                return;
            }
        }
    }

    public void ShipTeleport()
    {

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        transform.position += new Vector3(inputX, inputY, 0f) * teleportDistance;
    }
}
