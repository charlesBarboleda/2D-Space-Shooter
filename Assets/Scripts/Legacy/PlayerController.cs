using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    AbilityHolder abilityHolder;
    [SerializeField] Camera mainCamera;
    Vector2 moveInput;
    Rigidbody2D rb;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        abilityHolder = GetComponent<AbilityHolder>();
    }
    void Update()
    {
        Movement();
        Aim();
        CameraFollow();
        Abilities();


    }

    void Movement()
    {
        rb.velocity = moveInput * moveSpeed; // Movement
    }




    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); // Read the value of the input
    }

    void Aim()
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

    void CameraFollow()
    {
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -30f);


    }
    void Abilities()
    {
        // Use the Ability Laser
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // abilityHolder.abilities[0].TriggerAbility(gameObject, abilityHolder.target);

        }

        // Use the Ability Shield
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // abilityHolder.abilities[2].TriggerAbility(gameObject, transform);

        }

        // Use the Ability Teleport
        if (Input.GetKeyDown(KeyCode.E))
        {
            // abilityHolder.abilities[3].TriggerAbility(gameObject, abilityHolder.target);

        }


    }




}
