using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LacerateBullet : Bullet
{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {

        transform.localScale = new Vector3(1, 1, 1);
        transform.DOScale(50, 5f).SetEase(Ease.OutBack);
        Invoke(nameof(Deactivate), BulletLifetime);
        base.OnEnable();
        StartCoroutine(BulletFlashEffect());
    }
    void OnDisable()
    {
        transform.localScale = Vector3.zero;

    }

    IEnumerator BulletFlashEffect()
    {
        yield return new WaitForEndOfFrame();
        GameObject flash = ObjectPooler.Instance.SpawnFromPool("LacerateBulletFlash", transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        flash.SetActive(false);
    }
    protected override IEnumerator BulletOnHitEffect()
    {
        _bulletOnHitEffect = ObjectPooler.Instance.SpawnFromPool("LacerateBulletOnHitEffect", transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        _bulletOnHitEffect.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ThraxArmada") ||
        other.gameObject.CompareTag("Syndicates") ||
        other.gameObject.CompareTag("CrimsonFleet") ||
        other.gameObject.CompareTag("Asteroid"))
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            damageable.TakeDamage(BulletDamage);
            if (gameObject.CompareTag("PlayerBullet"))
            {
                EventManager.BulletDamageEvent(BulletDamage);
                EventManager.PlayerDamageDealtEvent(BulletDamage);
                if (shouldIncreaseCombo)
                    ComboManager.Instance.IncreaseCombo();
            }
            StartCoroutine(BulletOnHitEffect());
            UIManager.Instance.CreateOnHitDamageText(Mathf.Round(BulletDamage).ToString(), transform.position);

        }
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            if (shouldIncreaseCombo)
                ComboManager.Instance.IncreaseCombo();
            UIManager.Instance.CreateOnHitDamageText("1", transform.position);
            StartCoroutine(BulletOnHitEffect());
            other.gameObject.SetActive(false);
        }
    }

}



