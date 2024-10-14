using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserSettings : MonoBehaviour
{

    float _dps;
    int _comboCount = 0;
    int _maxComboCount = 5;



    private void OnTriggerStay2D(Collider2D other)
    {

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {

            if (other.CompareTag("ThraxArmada") || other.CompareTag("Syndicates") || other.CompareTag("CrimsonFleet") || other.CompareTag("Asteroid"))
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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Applies Corrode to the enemy if prestiged
        if (PlayerManager.Instance.chosenPrestige == PrestigeType.Plaguebringer)
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
    public float Dps { get => _dps; set => _dps = value; }




}
