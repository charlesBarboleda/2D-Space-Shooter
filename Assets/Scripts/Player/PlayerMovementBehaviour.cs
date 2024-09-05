using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBehaviour : MonoBehaviour
{
    Rigidbody2D _rb;
    Vector2 _moveInput;
    [SerializeField] float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }



    void Movement()
    {
        // Get the player's input

        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");


        _rb.velocity = _moveInput.normalized * moveSpeed;

    }

    public void SetMoveSpeed(float speed) => moveSpeed = speed;
    public float MoveSpeed() => moveSpeed;

}
