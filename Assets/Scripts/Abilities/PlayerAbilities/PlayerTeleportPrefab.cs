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
    }

    public void SetDamage(float dps)
    {
        this._dps = dps;
    }
}
