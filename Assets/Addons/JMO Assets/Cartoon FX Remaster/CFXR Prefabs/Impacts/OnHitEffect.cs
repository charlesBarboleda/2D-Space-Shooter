using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitEffect : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(Deactivate());
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
