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
        Debug.Log("Hit trigger name: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit player with gravity well");
            Vector2 direction = (transform.position - other.transform.position).normalized;
            rb.isKinematic = false;
            rb.AddForce(direction * _pullStrength);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        if (other.CompareTag("Player"))
        {
            rb.isKinematic = true;
        }

    }

    public float Duration { get => _duration; set => _duration = value; }
    public float PullStrength { get => _pullStrength; set => _pullStrength = value; }

}
