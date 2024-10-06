using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboManager : MonoBehaviour
{
    ComboManager _comboManager;
    int comboCount;
    PlayerManager _playerManager;
    GameObject damageEffect;


    void Start()
    {
        _comboManager = ComboManager.Instance;
        comboCount = _comboManager.comboCount;
        _playerManager = PlayerManager.Instance;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Pressed R");
            DealAOEDamage();
            Debug.Log("Spawned AOE");
        }
        if (comboCount >= 25)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                IncreasePlayerSpeed();
            }
        }
        else if (comboCount >= 50)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                IncreasePlayerBulletSpeed();
            }
        }
        else if (comboCount >= 75)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                IncreasePlayerHealth();
            }
        }
        else if (comboCount >= 125)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                IncreasePlayerDamage();
            }
        }
        else if (comboCount >= 250)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                IncreaseBulletAmount();
            }
        }
        else if (comboCount >= 500)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                DealAOEDamage();
            }
        }
    }

    void IncreasePlayerSpeed()
    {
        _playerManager.SetMoveSpeed(_playerManager.MoveSpeed() + 5f);
    }

    void IncreasePlayerBulletSpeed()
    {
        _playerManager.Weapon().bulletSpeed += 5f;
    }
    void IncreasePlayerDamage()
    {
        _playerManager.Weapon().bulletDamage += 200;
    }

    void IncreasePlayerHealth()
    {
        _playerManager.SetCurrentHealth(_playerManager.CurrentHealth() + 200);
        _playerManager.SetMaxHealth(_playerManager.MaxHealth() + 200);
    }

    void IncreaseBulletAmount()
    {
        _playerManager.Weapon().amountOfBullets += 3;
    }

    void DealAOEDamage()
    {
        GameObject damageEffect = ObjectPooler.Instance.SpawnFromPool("ComboAOE", transform.position, Quaternion.identity);
        damageEffect.transform.position = transform.position;
        Debug.Log("Spawned AOE");
        StartCoroutine(IncreaseAOEScale());
    }

    IEnumerator IncreaseAOEScale()
    {
        while (damageEffect.transform.localScale.x < 10)
        {
            damageEffect.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime;
            yield return null;
        }
        damageEffect.SetActive(false);
    }


    public void RemoveAllBuffs()
    {
        _playerManager.SetMoveSpeed(_playerManager.MoveSpeed() - 5f);
        _playerManager.Weapon().bulletDamage /= 2;
        _playerManager.SetMaxHealth(_playerManager.MaxHealth() - 200);
        _playerManager.SetCurrentHealth(_playerManager.CurrentHealth() - 200);
        _playerManager.Weapon().amountOfBullets -= 3;

    }
}
