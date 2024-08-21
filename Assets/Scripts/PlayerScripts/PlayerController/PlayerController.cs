using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

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
    void FixedUpdate()
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

    void Abilities()
    {
        // Switch between Abilities
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            abilityHolder.abilityIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            abilityHolder.abilityIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            abilityHolder.abilityIndex = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            abilityHolder.abilityIndex = 3;
        }

        // Use the Ability
        if (Input.GetKeyDown(KeyCode.Space))
        {
            abilityHolder.abilities[abilityHolder.abilityIndex].TriggerAbility(gameObject, abilityHolder.target);
        }

        //Cycle ability index using scroll wheel
        if (Input.mouseScrollDelta.y > 0)
        {
            abilityHolder.abilityIndex++;
            if (abilityHolder.abilityIndex > abilityHolder.abilities.Count - 1)
            {
                abilityHolder.abilityIndex = 0;
            }
        }
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
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10f);


    }




}
