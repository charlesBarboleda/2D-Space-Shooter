using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Faction))]
public class Building : MonoBehaviour
{

    float rotationSpeed;


    void Update()
    {
        // rotate the gameobject on the z axis
        transform.Rotate(0, 0, 10 * Time.deltaTime * rotationSpeed);
    }
    void OnEnable()
    {
        rotationSpeed *= Random.Range(0, 2) == 0 ? 1 : -1;
        StartCoroutine(SpawnAnimationWithDelay());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator SpawnAnimation()
    {
        GameObject animation = ObjectPooler.Instance.SpawnFromPool("BuildingSpawn", transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        animation.SetActive(false);
    }

    IEnumerator SpawnAnimationWithDelay()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(SpawnAnimation());
    }


    public void TeleportAway()
    {
        GameManager.Instance.RemoveEnemy(gameObject);
        StartCoroutine(TeleportEffect());

    }

    IEnumerator TeleportEffect()
    {

        yield return StartCoroutine(SpawnAnimation());
        gameObject.SetActive(false);
    }


}
