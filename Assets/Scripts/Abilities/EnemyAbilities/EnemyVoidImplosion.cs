using System;
using System.Collections;
using UnityEngine;

public class EnemyVoidImplosion : MonoBehaviour
{
    [SerializeField] ParticleSystem _implosionParticles;
    [SerializeField] ParticleSystem _chargingParticles;
    [SerializeField] float _countdown;
    [SerializeField] float _size;
    [SerializeField] float _damage;

    private float _initialSize;

    void OnEnable()
    {
        // Log messages to track behavior
        Debug.Log("OnEnable called for " + gameObject.name);

        if (_implosionParticles == null)
        {
            Debug.LogError("Implosion particles are missing or destroyed!");
            return;
        }

        if (_chargingParticles == null)
        {
            Debug.LogError("Charging particles are missing or destroyed!");
            return;
        }

        // Randomly set the initial size and countdown duration
        _size = UnityEngine.Random.Range(5, 30);
        _countdown = _size * 0.1f;
        _initialSize = _size;

        // Set the implosion particle size
        _implosionParticles.transform.localScale = new Vector3(_size / 2, _size / 2, _size / 2);

        // Set the charging particle size to its maximum initially
        _chargingParticles.transform.localScale = new Vector3(_size, _size, _size);

        // Start the countdown coroutine
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        // Check if particles still exist
        if (_implosionParticles == null)
        {
            Debug.Log("Implosion particles were destroyed or null!0");
        }

        // Play the charging particles
        _chargingParticles.Play();

        float timeElapsed = 0f;

        // Gradually shrink the charging particle size over the countdown duration
        while (timeElapsed < _countdown)
        {
            if (_implosionParticles == null)
            {
                Debug.LogError("Implosion particles were destroyed mid-countdown!");
                yield break;
            }

            timeElapsed += Time.deltaTime;

            // Calculate the current size based on the remaining countdown time
            float newSize = Mathf.Lerp(_size, 0, timeElapsed / _countdown);
            _chargingParticles.transform.localScale = new Vector3(newSize, newSize, newSize);

            yield return null;  // Wait for the next frame
        }

        // Stop the charging particles when the countdown finishes
        _chargingParticles.Stop();

        // Trigger the implosion
        StartCoroutine(Implode());
    }

    IEnumerator Implode()
    {
        // Check if _implosionParticles exists before trying to access it
        if (_implosionParticles != null)
        {
            _implosionParticles.Play();
            yield return new WaitForSeconds(2.2f);
            _implosionParticles.Stop();
        }
        else
        {
            Debug.LogError("Implosion particles were null before implosion!");
        }

        // Deactivate the GameObject only after the implosion effect has finished
        gameObject.SetActive(false);
    }
    // Properties for Damage, Countdown, and Size
    public float Damage { get => _damage; set => _damage = value; }
    public float Countdown { get => _countdown; set => _countdown = value; }
    public float Size { get => _size; set => _size = value; }
}
