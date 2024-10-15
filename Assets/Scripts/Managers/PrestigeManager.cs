using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class PrestigeManager : MonoBehaviour
{
    [SerializeField] Sprite _lifewarden;
    [SerializeField] Sprite _plaguebringer;
    [SerializeField] Sprite _sunlancer;
    [SerializeField] GameObject lifewardenBuff;
    [SerializeField] GameObject plaguebringerBuff;
    [SerializeField] GameObject sunlancerBuff;
    public PrestigeType chosenPrestige = PrestigeType.None;
    SpriteRenderer _spriteRenderer;
    Weapon _weapon;
    PickUpBehaviour _pickUpBehaviour;
    PlayerMovementBehaviour _movement;
    PlayerHealthBehaviour _health;


    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _weapon = GetComponent<Weapon>();
        _pickUpBehaviour = GetComponent<PickUpBehaviour>();
        _movement = GetComponent<PlayerMovementBehaviour>();
        _health = GetComponent<PlayerHealthBehaviour>();

    }

    void OnEnable()
    {
        EventManager.OnPrestigeChange += ApplyPrestigeEffects;
    }

    void OnDisable()
    {
        EventManager.OnPrestigeChange -= ApplyPrestigeEffects;
    }
    public void PrestigeTo(PrestigeType prestige)
    {
        chosenPrestige = prestige;
        EventManager.PrestigeChangeEvent(prestige);
    }
    public void PrestigeToPlaguebringer()
    {
        StartCoroutine(PrestigeAnimation(PrestigeType.Plaguebringer, "PlaguebringerPrestigeAnimation", 1.3f));
    }
    public void PrestigeToLifewarden()
    {
        StartCoroutine(PrestigeAnimation(PrestigeType.Lifewarden, "LifewardenPrestigeAnimation", 1.3f));
    }

    public void PrestigeToSunlancer()
    {
        StartCoroutine(PrestigeAnimation(PrestigeType.Sunlancer, "SunlancerPrestigeAnimation", 1.3f));
    }

    IEnumerator PrestigeAnimation(PrestigeType prestigeTo, string animationName, float animationDuration)
    {
        GameObject prestigeAnimation = ObjectPooler.Instance.SpawnFromPool(animationName, transform.position, Quaternion.identity);
        prestigeAnimation.transform.SetParent(transform);
        yield return new WaitForSeconds(2f);
        if (prestigeTo == PrestigeType.Plaguebringer)
        {
            UIManager.Instance.PlaguebringerPrestigeCrackAndShatter();
        }
        else if (prestigeTo == PrestigeType.Lifewarden)
        {
            UIManager.Instance.LifewardenPrestigeCrackAndShatter();
        }
        else if (prestigeTo == PrestigeType.Sunlancer)
        {
            UIManager.Instance.SunlancerPrestigeCrackAndShatter();
        }
        yield return new WaitForSeconds(animationDuration);
        prestigeAnimation.SetActive(false);
        PrestigeTo(prestigeTo);
    }

    void ApplyPrestigeEffects(PrestigeType prestige)
    {
        switch (prestige)
        {
            case PrestigeType.None:
                break;
            case PrestigeType.Plaguebringer:
                plaguebringerBuff.SetActive(true);
                _spriteRenderer.sprite = _plaguebringer;
                _spriteRenderer.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
                _weapon.weaponType = WeaponType.PlayerCorrodeBullet;
                _weapon.amountOfBullets = 1;
                _weapon.bulletLifetime += 5f;
                _weapon.bulletSpeed -= 10f;
                _weapon.fireRate = 3f;
                _pickUpBehaviour.PickUpRadius += 15f;
                _movement.SetMoveSpeed(-5f);
                _health.SetMaxHealth(250f);
                _health.SetCurrentHealth(250f);
                StartCoroutine(PlayerCorrosion());
                break;
            case PrestigeType.Lifewarden:
                lifewardenBuff.SetActive(true);
                _spriteRenderer.sprite = _lifewarden;
                _spriteRenderer.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
                _weapon.weaponType = WeaponType.PlayerLeechBullet;
                _weapon.amountOfBullets = 5;
                _weapon.bulletSpeed -= 5;
                _weapon.fireRate = 0.25f;
                _weapon.shootingAngle = 20f;
                _pickUpBehaviour.PickUpRadius += 10f;
                _movement.SetMoveSpeed(-5f);
                _health.SetMaxHealth(500f);
                _health.SetCurrentHealth(500f);
                _health.SetHealthRegenRate(25f);
                break;
            case PrestigeType.Sunlancer:
                _spriteRenderer.sprite = _sunlancer;
                _spriteRenderer.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
                _weapon.weaponType = WeaponType.PlayerReflectBullet;
                _weapon.amountOfBullets = 10;
                _weapon.bulletSpeed += 20;
                _weapon.fireRate = 1f;
                _pickUpBehaviour.PickUpRadius += 20f;
                _movement.SetMoveSpeed(15f);
                _health.SetMaxHealth(100f);
                _health.SetCurrentHealth(100f);
                break;


        }
    }
    IEnumerator PlayerCorrosion()
    {
        while (chosenPrestige == PrestigeType.Plaguebringer)
        {
            GameObject corrode = ObjectPooler.Instance.SpawnFromPool("CorrodeEffect", transform.position, Quaternion.identity);
            corrode.GetComponent<CorrosionEffect>().ApplyCorrode(gameObject, _weapon.bulletDamage * 5);
            yield return new WaitForSeconds(5f);
        }
    }
}
