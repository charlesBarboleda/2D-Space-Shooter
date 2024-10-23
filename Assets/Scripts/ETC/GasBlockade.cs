using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GasBlockade : MonoBehaviour
{
    Kinematics kinematics;
    NavMeshAgent navMeshAgent;

    [SerializeField] bool _shouldDisable = true;
    void OnEnable()
    {
        if (_shouldDisable)
            StartCoroutine(DisableAfterTime());
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        kinematics = other.GetComponent<Kinematics>();
        navMeshAgent = other.GetComponent<NavMeshAgent>();
        if (damageable != null)
        {
            if (other.CompareTag("Player"))
            {
                damageable.TakeDamage(10f);
            }
        }
        if (kinematics != null)
        {
            kinematics.outOfBounds = true;
        }
        if (navMeshAgent != null)
            navMeshAgent.enabled = false;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (kinematics != null)
        {
            kinematics.outOfBounds = false;
        }
        if (navMeshAgent != null)
            navMeshAgent.enabled = true;
    }

    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(30f);
        gameObject.SetActive(false);
    }
}
