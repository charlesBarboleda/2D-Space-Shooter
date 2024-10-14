using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Turrets")]
public class AbilityTurrets : Ability
{
    public float bulletDamage;
    public float fireRate;

    public override void AbilityLogic(GameObject owner, Transform target, bool isUltimate = false)
    {
        if (isUltimate)
        {
            UIManager.Instance.UltimateActivateCrackAndShatter();
            GameManager.Instance.StartCoroutine(UltimateTurretLogic());
        }

    }

    IEnumerator UltimateTurretLogic()
    {
        yield return new WaitForSeconds(1f);
        PlayerManager.GetInstance().GetComponent<TurretManager>().SetBulletSpeed(20);
        PlayerManager.GetInstance().GetComponent<TurretManager>().AddBulletAmount(2);
        PlayerManager.GetInstance().GetComponent<TurretManager>().SetTurretFireRate(PlayerManager.GetInstance().GetComponent<TurretManager>().GetTurretFireRate() / 2f);
        PlayerManager.GetInstance().GetComponent<TurretManager>().SetTurretDamage(PlayerManager.GetInstance().GetComponent<TurretManager>().GetTurretDamage() * 2);
        yield return new WaitForSeconds(ultimateDuration);
        PlayerManager.GetInstance().GetComponent<TurretManager>().SetBulletSpeed(-20);
        PlayerManager.GetInstance().GetComponent<TurretManager>().AddBulletAmount(-2);
        PlayerManager.GetInstance().GetComponent<TurretManager>().SetTurretFireRate(PlayerManager.GetInstance().GetComponent<TurretManager>().GetTurretFireRate() * 2f);
        PlayerManager.GetInstance().GetComponent<TurretManager>().SetTurretDamage(PlayerManager.GetInstance().GetComponent<TurretManager>().GetTurretDamage() / 2);
    }


    public override void ResetStats()
    {
        bulletDamage = 10f;
        fireRate = 0.4f;
        isUnlocked = false;
    }
}
