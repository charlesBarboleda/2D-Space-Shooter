using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 1f;
    [SerializeField] Camera mainCamera;
    Vector3 movement;

    void FixedUpdate()
    {

        Movement();
        Aim();
        CameraFollow();
    }



    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = new Vector3(horizontal, vertical, 0f).normalized;

        transform.position += movement * Time.deltaTime * playerSpeed;
    }

    private void Aim()
    {
        // Get the world position of the mouse cursor
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the GameObject to the mouse position
        Vector3 direction = mousePosition - transform.position;

        // Calculate the angle in radians and convert to degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the GameObject
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 270f));
    }

    private void CameraFollow()
    {
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);


    }




}
