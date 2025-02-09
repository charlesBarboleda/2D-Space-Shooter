using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrestigeManager : MonoBehaviour
{
    [SerializeField] Sprite _lifewarden;
    [SerializeField] Sprite _plaguebringer;
    [SerializeField] Sprite _sunlancer;
    [SerializeField] Sprite _berzerker;
    [SerializeField] GameObject lifewardenBuff;
    [SerializeField] GameObject plaguebringerBuff;
    [SerializeField] GameObject sunlancerBuff;
    [SerializeField] GameObject _berzerkerBuff;
    [SerializeField] AudioClip _onPrestigeAudio;
    bool hasPrestiged = false;
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
        if (!hasPrestiged)
        {
            hasPrestiged = true;
            chosenPrestige = prestige;
            EventManager.PrestigeChangeEvent(prestige);
        }
        else
        {
            UIManager.Instance.MidScreenWarningText("You have already prestiged!", 2f);
        }
    }
    public void PrestigeToPlaguebringer()
    {
        if (!hasPrestiged) // Add this check to prevent animation from starting
        {
            StartCoroutine(PrestigeAnimation(PrestigeType.Plaguebringer, "PlaguebringerPrestigeAnimation", 1.2f));
        }
        else
        {
            UIManager.Instance.MidScreenWarningText("You have already prestiged!", 2f);
        }
    }

    public void PrestigeToLifewarden()
    {
        if (!hasPrestiged)
        {
            StartCoroutine(PrestigeAnimation(PrestigeType.Lifewarden, "LifewardenPrestigeAnimation", 1.2f));
        }
        else
        {
            UIManager.Instance.MidScreenWarningText("You have already prestiged!", 2f);
        }
    }

    public void PrestigeToSunlancer()
    {
        if (!hasPrestiged)
        {
            StartCoroutine(PrestigeAnimation(PrestigeType.Sunlancer, "SunlancerPrestigeAnimation", 1.2f));
        }
        else
        {
            UIManager.Instance.MidScreenWarningText("You have already prestiged!", 2f);
        }
    }

    public void PrestigeToBerzerker()
    {
        if (!hasPrestiged)
        {
            StartCoroutine(PrestigeAnimation(PrestigeType.Berzerker, "BerzerkerPrestigeAnimation", 1.2f));
        }
        else
        {
            UIManager.Instance.MidScreenWarningText("You have already prestiged!", 2f);
        }
    }


    public bool HasAlreadyPrestiged()
    {
        return hasPrestiged;
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
        else if (prestigeTo == PrestigeType.Berzerker)
        {
            UIManager.Instance.BerzerkerPrestigeCrackAndShatter();
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
                _spriteRenderer.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
                _weapon.weaponType = WeaponType.PlayerCorrodeBullet;
                _weapon.amountOfBullets = 1;
                _weapon.bulletLifetime += 5f;
                _weapon.bulletSpeed -= 10f;
                _weapon.fireRate += 3f;
                _pickUpBehaviour.PickUpRadius += 15f;
                _movement.moveSpeed -= -5f;
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
                _weapon.fireRate += 0.5f;
                _weapon.bulletDamage += 50;
                _weapon.shootingAngle = 20f;
                _pickUpBehaviour.PickUpRadius += 10f;
                _movement.moveSpeed -= -5f;
                _health.SetMaxHealth(500f);
                _health.SetCurrentHealth(500f);
                _health.SetHealthRegenRate(25f);
                break;
            case PrestigeType.Sunlancer:
                sunlancerBuff.SetActive(true);
                _spriteRenderer.sprite = _sunlancer;
                _spriteRenderer.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                _weapon.weaponType = WeaponType.PlayerReflectBullet;
                _weapon.bulletDamage *= 2;
                _weapon.bulletSpeed += 20;
                _weapon.fireRate += 0.5f;
                _pickUpBehaviour.PickUpRadius += 20f;
                _movement.moveSpeed *= 2;
                _health.SetMaxHealth(100f);
                _health.SetCurrentHealth(100f);
                _health.healthCap = 0.5f;
                break;
            case PrestigeType.Berzerker:
                _berzerkerBuff.SetActive(true);
                _spriteRenderer.sprite = _berzerker;
                _spriteRenderer.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                _weapon.weaponType = WeaponType.PlayerLacerateBullet;
                _weapon.amountOfBullets = 1;
                _weapon.bulletDamage += 100;
                _weapon.bulletLifetime = 10f;
                _weapon.fireRate += 1f;
                _weapon.bulletSpeed += 10;
                _pickUpBehaviour.PickUpRadius += 15f;
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
