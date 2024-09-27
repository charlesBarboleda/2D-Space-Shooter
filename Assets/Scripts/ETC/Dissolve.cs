using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    Material dissolveMaterial;
    public float dissolveAmount = 0;
    public float dissolveSpeed = 0.2f;

    void Start()
    {
        dissolveMaterial = GetComponent<SpriteRenderer>().material;
    }
    public IEnumerator DissolveOut()
    {
        while (dissolveAmount < 1)
        {
            dissolveAmount += Time.deltaTime * dissolveSpeed;
            dissolveMaterial.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
        }
    }

    public IEnumerator DissolveIn()
    {
        while (dissolveAmount > 0)
        {
            dissolveAmount -= Time.deltaTime * dissolveSpeed;
            dissolveMaterial.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
        }
    }
}
