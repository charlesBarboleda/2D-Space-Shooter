using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Faction))]
public class Building : MonoBehaviour
{

    void OnEnable()
    {

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
        SpawnerManager.Instance.RemoveEnemy(gameObject);
        StartCoroutine(TeleportEffect());

    }

    IEnumerator TeleportEffect()
    {

        yield return StartCoroutine(SpawnAnimation());
        gameObject.SetActive(false);
    }


}
