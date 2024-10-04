using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasBlockade : MonoBehaviour
{
    [SerializeField] bool _shouldDisable = true;
    void OnEnable()
    {
        if (_shouldDisable)
            StartCoroutine(DisableAfterTime());
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        Kinematics kinematics = other.GetComponent<Kinematics>();
        if (damageable != null)
        {
            if (other.CompareTag("Player"))
            {
                damageable.TakeDamage(1f);
            }
        }
        if (kinematics != null)
        {
            kinematics.outOfBounds = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Kinematics kinematics = other.GetComponent<Kinematics>();
        if (kinematics != null)
        {
            kinematics.outOfBounds = false;
        }
    }

    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(30f);
        gameObject.SetActive(false);
    }
}
