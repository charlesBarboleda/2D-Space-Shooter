using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldPrefab : MonoBehaviour
{
    float _dps;
    PlayerHealthBehaviour _playerHealth;
    int _maxComboCount = 20;
    int _comboCount = 0;
    void OnEnable()
    {
        _playerHealth = PlayerManager.Instance.GetComponent<PlayerHealthBehaviour>();
        _playerHealth.isDead = true;
    }
    void OnTriggerStay2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            if (other.CompareTag("ThraxArmada") || other.CompareTag("Syndicates") || other.CompareTag("CrimsonFleet"))
            {
                UIManager.Instance.CreateOnHitDamageText(Mathf.Round(_dps).ToString(), other.transform.position);
                damageable.TakeDamage(_dps);
                if (_comboCount < _maxComboCount)
                {
                    _comboCount++;
                }
                else
                {
                    _comboCount = 0;
                    ComboManager.Instance.IncreaseCombo();
                }
            }

        }
        if (other.CompareTag("EnemyBullet"))
        {
            other.gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        _playerHealth.isDead = false;
    }

    public float Dps { get => _dps; set => _dps = value; }

}
