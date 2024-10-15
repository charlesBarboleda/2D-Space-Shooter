using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleportPrefab : MonoBehaviour
{
    private float _dps;
    private int _comboCount = 0;
    private int _maxComboCount = 20;
    private void OnTriggerStay2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            if (other.CompareTag("ThraxArmada") || other.CompareTag("Syndicates") || other.CompareTag("CrimsonFleet") || other.CompareTag("Asteroid"))
            {
                UIManager.Instance.CreateOnHitDamageText(Mathf.Round(_dps).ToString(), other.transform.position);
                damageable.TakeDamage(_dps);
                if (PlayerManager.Instance.PrestigeManager().chosenPrestige == PrestigeType.Lifewarden)
                {
                    // Heal the player for 5% of the damage dealt if prestiged
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
    }
    void OnTriggerEnter2D(Collider2D other)
    {
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



    public void SetDamage(float dps)
    {
        this._dps = dps;
    }
}
