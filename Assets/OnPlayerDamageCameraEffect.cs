using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OnPlayerDamageCameraEffect : MonoBehaviour
{
    public static OnPlayerDamageCameraEffect Instance { get; private set; }
    [SerializeField] Volume _volume;
    Vignette _vignette;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        _volume = GetComponent<Volume>();
        if (_volume.profile.TryGet(out Vignette vignette))
        {
            _vignette = vignette;
        }
    }
    public void DamageEffect()
    {
        StopAllCoroutines();
        StartCoroutine(DamageEffectCoroutine());
    }

    IEnumerator DamageEffectCoroutine()
    {
        // Increase the _vignette intensity to simulate damage
        float currentIntensity = _vignette.intensity.value;
        while (currentIntensity < 0.5f)
        {
            currentIntensity += Time.deltaTime * 3f;
            _vignette.intensity.value = Mathf.Clamp(currentIntensity, 0, 0.5f);
            yield return null;
        }

        // Hold the _vignette at max intensity for a brief moment
        yield return new WaitForSeconds(0.1f);

        // Gradually decrease the _vignette intensity back to normal
        while (currentIntensity > 0)
        {
            currentIntensity -= Time.deltaTime * 3f;
            _vignette.intensity.value = Mathf.Clamp(currentIntensity, 0, 0.5f);
            yield return null;
        }
    }
}
