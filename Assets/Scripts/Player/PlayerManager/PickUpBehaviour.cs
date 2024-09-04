using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBehaviour
{
    public float pickUpRadius { get; private set; }

    public PickUpBehaviour(float pickUpRadius)
    {
        this.pickUpRadius = pickUpRadius;
    }

    void PickUpItems(Vector2 position)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, pickUpRadius, LayerMask.GetMask("Pickable"));

        // Iterate over each collider and trigger attraction
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Pickable"))
                if (hit.GetComponent<CurrencyDrop>() != null)
                    hit.GetComponent<CurrencyDrop>().isAttracted = true;

        }
    }

    public void SetPickUpRadius(float radius)
    {
        pickUpRadius = radius;
    }
}
