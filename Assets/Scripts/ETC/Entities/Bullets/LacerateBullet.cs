using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LacerateBullet : Bullet
{
    SpriteRenderer _spriteRenderer;
    protected override void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.zero;
        base.Awake();
    }

    protected override void OnEnable()
    {

        transform.localScale = new Vector3(0, 0, 0);
        if (_spriteRenderer != null) _spriteRenderer.color = Color.yellow;
        if (_spriteRenderer != null) _spriteRenderer.DOColor(Color.red, 5f);
        transform.DOScaleY(5f, 5f);
        transform.DOScaleX(10f, 5f);
        Invoke(nameof(Deactivate), 10f);
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
            StartCoroutine(BulletOnHitEffect());
            UIManager.Instance.CreateOnHitDamageText(Mathf.Round(BulletDamage).ToString(), transform.position);
            if (shouldIncreaseCombo)
                ComboManager.Instance.IncreaseCombo();
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



