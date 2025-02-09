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
            if (other.CompareTag("ThraxArmada") || other.CompareTag("Syndicates") || other.CompareTag("CrimsonFleet") || other.CompareTag("Asteroid"))
            {
                UIManager.Instance.CreateOnHitDamageText(Mathf.Round(_dps).ToString(), other.transform.position);
                damageable.TakeDamage(_dps);
                EventManager.PlayerDamageDealtEvent(_dps);

                if (PlayerManager.Instance.PrestigeManager().chosenPrestige == PrestigeType.Lifewarden)
                {
                    // Heal the player for 1% of the damage dealt if prestiged
                    PlayerManager.Instance.GetComponent<PlayerHealthBehaviour>().currentHealth += _dps * 0.05f;
                }
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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            Bullet script = other.GetComponent<Bullet>();
            EventManager.ShieldAbsorbEvent(script.BulletDamage);
        }
        // Applies Corrode to the enemy if prestiged
        if (PlayerManager.Instance.PrestigeManager().chosenPrestige == PrestigeType.Plaguebringer)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                if (other.CompareTag("ThraxArmada") || other.CompareTag("Syndicates") || other.CompareTag("CrimsonFleet") || other.CompareTag("Asteroid"))
                {
                    GameObject Corrode = ObjectPooler.Instance.SpawnFromPool("CorrodeEffect", other.transform.position, Quaternion.identity);
                    Corrode.GetComponent<CorrosionEffect>().ApplyCorrode(other.gameObject, _dps * 10);

                }
            }
        }
    }


    void OnDisable()
    {
        _playerHealth.isDead = false;
    }

    public float Dps { get => _dps; set => _dps = value; }

}
