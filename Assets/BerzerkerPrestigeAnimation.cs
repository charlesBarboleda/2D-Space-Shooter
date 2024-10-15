using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerzerkerPrestigeAnimation : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;

    void OnEnable()
    {
        StartCoroutine(PrestigeAnimation(1.5f));
    }

    IEnumerator PrestigeAnimation(float duration)
    {
        var shape = _particleSystem.shape;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            shape.radius = Mathf.Lerp(50, 0, time / duration);
            yield return null;
        }
    }
}
