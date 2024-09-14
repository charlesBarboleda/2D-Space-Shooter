using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Faction))]
public class CargoShip : MonoBehaviour
{
    Health _health;
    [SerializeField] GameObject spawnAnimation;

    void Awake()
    {
        _health = GetComponent<Health>();
    }

    void OnEnable()
    {
        StartCoroutine(StartSpawnAnimationWithDelay());
    }

    IEnumerator StartSpawnAnimationWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject animation = Instantiate(spawnAnimation, transform.position, Quaternion.identity);
        Destroy(animation, 1f);
    }

    public void TeleportAway()
    {
        GameObject animation = Instantiate(spawnAnimation, transform.position, Quaternion.identity);
        Destroy(animation, 1f);
        gameObject.SetActive(false);
    }

    public Health Health { get => _health; }
}
