using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour, IDamageable
{
    int hitsToBreak = 3;
    [SerializeField] List<Transform> _targets;
    [SerializeField] float _speed = 5f;


    void OnEnable()
    {

    }
    void Update()
    {
        int RandomTarget = Random.Range(0, _targets.Count);
        transform.position = Vector3.MoveTowards(transform.position, _targets[RandomTarget].position, _speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _targets[RandomTarget].position) < 1f)
        {
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float damage)
    {
        CameraShake.Instance.TriggerShake(_cameraShakeMagnitude, _cameraShakeDuration);
        hitsToBreak--;
        if (hitsToBreak <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // Hide the ship visually and start the death animation coroutine
        StartCoroutine(HandleDeath());
    }

    public IEnumerator HandleDeath()
    {

        isDead = true;

        // Disable the turrets if there are any
        if (turretChildren.Count > 0) turretChildren.ForEach(child => child.SetActive(false));

        // Stops the exhaust particles
        exhaustChildren.ForEach(child => child.SetActive(false));

        // Disable all colliders
        _colliders.ForEach(collider => collider.enabled = false);

        // Hide the ship's sprite
        _spriteRenderer.enabled = false;


        // Drop the power up


        // Wait for the death animation to complete
        yield return StartCoroutine(DeathAnimation());

        // After the animation is done, deactivate the entire GameObject
        gameObject.SetActive(false);

    }

    public IEnumerator DeathAnimation()
    {
        GameObject exp = ObjectPooler.Instance.SpawnFromPool(_deathExplosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f); // Wait for the animation to finish
        exp.SetActive(false);

    }
    public float Speed { get => _speed; set => _speed = value; }
    public int HitsToBreak { get => hitsToBreak; set => hitsToBreak = value; }


}
