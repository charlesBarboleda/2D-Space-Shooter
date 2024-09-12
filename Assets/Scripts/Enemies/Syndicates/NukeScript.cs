using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nuke : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private SpriteRenderer nukeSpriteRenderer;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject target;
    public float nukeGrowthRate = 0.5f;
    public float maxNukeSize = 4.5f;






    void Start()
    {
        target.transform.localScale = new Vector3(0, 0, 0);
        nukeSpriteRenderer.color = new Color(255, 0, 0, 0.1f);
        ActivateNukeCountdown();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target.transform.localScale.x < maxNukeScale(maxNukeSize).x)
        {
            target.transform.localScale += new Vector3(nukeGrowthRate, nukeGrowthRate, nukeGrowthRate);
        }
        if (target.transform.localScale.x >= maxNukeScale(maxNukeSize).x)
        {
            Explode();
        }
    }

    private Vector3 maxNukeScale(float maxScale)
    {
        return new Vector3(maxScale, maxScale, maxScale);
    }


    void OnEnable()
    {
        ActivateNukeCountdown();
    }

    void ActivateNukeCountdown()
    {
        targetTransform.localScale = new Vector3(maxNukeSize, maxNukeSize, maxNukeSize);
        target.transform.localScale = new Vector3(0, 0, 0);

    }

    void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 0.25f);
        Destroy(gameObject);
    }
}
