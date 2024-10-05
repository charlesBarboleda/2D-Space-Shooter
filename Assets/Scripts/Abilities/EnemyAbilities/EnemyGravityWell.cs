using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGravityWell : MonoBehaviour
{

    [SerializeField] float _duration;
    [SerializeField] float _pullStrength = 3000f;


    void OnEnable()
    {
        Invoke("DisableGravityWell", _duration);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void DisableGravityWell()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        Kinematics kinematics = other.GetComponent<Kinematics>();

        if (other.CompareTag("Player"))
        {

            Vector2 direction = (transform.position - other.transform.position).normalized;
            rb.isKinematic = false;
            kinematics.ShouldMove = false;
            rb.AddForce(direction * _pullStrength);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        Kinematics kinematics = other.GetComponent<Kinematics>();

        if (other.CompareTag("Player"))
        {
            kinematics.ShouldMove = true;
            rb.isKinematic = true;
        }

    }

    public float Duration { get => _duration; set => _duration = value; }
    public float PullStrength { get => _pullStrength; set => _pullStrength = value; }

}
