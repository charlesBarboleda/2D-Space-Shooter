using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrosionDebuff : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(DisableInTime(5f));
    }

    IEnumerator DisableInTime(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
