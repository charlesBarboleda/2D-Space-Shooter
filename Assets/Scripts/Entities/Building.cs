using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Faction))]
public class Building : MonoBehaviour
{

    Faction _faction;



    void Awake()
    {
        _faction = GetComponent<Faction>();
    }
    void OnEnable()
    {
        StartCoroutine(SpawnAnimationWithDelay());
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
        GameManager.Instance.RemoveEnemy(gameObject, _faction);
        StartCoroutine(TeleportEffect());

    }

    IEnumerator TeleportEffect()
    {

        yield return StartCoroutine(SpawnAnimation());
        gameObject.SetActive(false);
    }


}
