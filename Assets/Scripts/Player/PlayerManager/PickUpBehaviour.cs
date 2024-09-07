using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBehaviour : MonoBehaviour
{
    [SerializeField] string _pickUpType;
    [SerializeField] public float pickUpRadius = 2f;
    float _attractionSpeed { get; set; } = 50f;

    // Update is called once per frame
    void Update()
    {
        PickUpLogic();
    }

    void PickUpLogic()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickUpRadius, LayerMask.GetMask("Pickable"));

        // Iterate over each collider and trigger attraction
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Debris") || hit.CompareTag("PowerUp"))
            {
                hit.GetComponent<IPickable>().maxSpeed = _attractionSpeed;
                hit.GetComponent<IPickable>().isAttracted = true;
            }
        }
    }

    public float PickUpRadius() => pickUpRadius;
    public float SetPickUpRadius(float newRadius) => pickUpRadius = newRadius;

}
