using System.Collections;
using UnityEngine;

public class EnemyGasExplosion : MonoBehaviour
{
    GameObject _circleTargetCountdown;
    [SerializeField] ParticleSystem _explosionParticles;
    [SerializeField] ParticleSystem _chargingParticles;

    [SerializeField] float _countdown = 5f;  // Default countdown duration
    [SerializeField] float _size = 50f;      // Default size
    [SerializeField] float _damage = 10000f; // Default damage

    private float _initialSize;
    private bool isActivated = false;

    void OnEnable()
    {
        ResetExplosion(); // Initialize and reset everything
        StartCoroutine(DelayedCircleCountdown());
    }

    IEnumerator DelayedCircleCountdown()
    {


        // Wait until the position is confirmed to be non-zero or as expected
        yield return new WaitForSeconds(0.5f);

        // Recheck the position before starting the circle countdown

        if (transform.position != Vector3.zero || isCenterPosition())
        {
            // Reset the circle position to ensure it's not at (0,0,0)
            _circleTargetCountdown.transform.position = transform.position;

            StartCoroutine(CircleCountdown());
        }
        else
        {
            Debug.LogError("Position is invalid for spawning!");
        }
    }

    private bool isCenterPosition()
    {
        // Consider a small threshold for floating point precision issues
        const float threshold = 0.1f;
        bool isCenter = Mathf.Abs(transform.position.x) < threshold && Mathf.Abs(transform.position.y) < threshold;
        return isCenter;
    }

    private void ResetExplosion()
    {
        // Ensure particles are stopped and the initial size is set
        _chargingParticles.Stop();
        _explosionParticles.Stop();

        if (_explosionParticles == null || _chargingParticles == null)
        {
            Debug.LogError("Particles are missing or destroyed!");
            return;
        }

        // Log the position to ensure it is set correctly


        // Create or reset the countdown circle at the object's position
        _circleTargetCountdown = ObjectPooler.Instance.SpawnFromPool("TargetHitCircle", transform.position, Quaternion.identity);

        // Ensure that the target circle position matches the explosion position
        _circleTargetCountdown.transform.position = transform.position;
        _circleTargetCountdown.transform.localScale = Vector3.zero;

        // Set the initial size and countdown duration
        _initialSize = _size;
        _countdown = _initialSize * 0.1f;

        // Set the particle sizes
        _explosionParticles.transform.localScale = new Vector3(_initialSize / 2.1f, _initialSize / 2.1f, _initialSize / 2.1f);
        _chargingParticles.transform.localScale = new Vector3(_initialSize / 3.5f, _initialSize / 3.5f, _initialSize / 3.5f);

    }

    IEnumerator CircleCountdown()
    {
        float timeElapsed = 0f;
        while (timeElapsed < _countdown)
        {
            timeElapsed += Time.deltaTime;
            float lerpValue = timeElapsed / _countdown;
            float currentSize = Mathf.Lerp(0, _initialSize - 5f, lerpValue);

            // Keep the circle in sync with the explosion's position
            _circleTargetCountdown.transform.position = transform.position;
            _circleTargetCountdown.transform.localScale = new Vector3(currentSize, currentSize, currentSize);

            yield return null;
        }
        _circleTargetCountdown.SetActive(false);
        yield return StartCoroutine(ChargingParticles());
    }

    IEnumerator ChargingParticles()
    {
        _chargingParticles.Play();
        yield return new WaitForSeconds(1f);
        StartCoroutine(Implode());
        _chargingParticles.Stop();
    }

    IEnumerator Implode()
    {

        LayerMask _damageLayerMask = LayerMask.GetMask("Player", "CrimsonFleet", "Syndicates");
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _initialSize, _damageLayerMask);

        // Apply damage or effects to all hit objects
        foreach (Collider2D hitCollider in hitColliders)
        {
            IDamageable damageable = hitCollider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(_damage);
            }
        }

        // Play the explosion particles and wait for them to finish
        if (_explosionParticles != null)
        {
            _explosionParticles.Play();
            ObjectPooler.Instance.SpawnFromPool("GasBlockadeLarge", transform.position, Quaternion.identity);
            yield return new WaitForSeconds(_explosionParticles.main.duration);
            _explosionParticles.Stop();
        }
        else
        {
            Debug.LogError("Explosion particles are null before explosion!");
        }

        // Deactivate the GameObject after the explosion effect is complete
        gameObject.SetActive(false);
    }

    // Properties for Damage, Countdown, and Size
    public float Damage { get => _damage; set => _damage = value; }
    public float Countdown { get => _countdown; set => _countdown = value; }
    public float Size { get => _size; set => _size = value; }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _initialSize);
    }
}
