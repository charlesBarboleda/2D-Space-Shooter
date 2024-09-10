using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EscortObjective", menuName = "Objectives/EscortObjective", order = 1)]
public class EscortObjective : Objective
{
    [SerializeField] int currentCheckpoints;
    [SerializeField] int requiredCheckpoints;
    [SerializeField] float distancePerCheckpoint = 5f;
    [SerializeField] float shipHealth;
    [SerializeField] List<Vector3> escortPathways;


    GameObject escortShip;
    public override void InitObjective()
    {
        currentCheckpoints = 0;
        requiredCheckpoints = escortPathways.Count - 1;
        escortShip = ObjectPooler.Instance.SpawnFromPool("CargoShip", escortPathways[0], Quaternion.identity);
        GameManager.Instance.AddEnemy(escortShip);
        CargoShip escortShipScript = escortShip.GetComponent<CargoShip>();
        escortShipScript.SetHealth(shipHealth);
        SetIsCompleted(false);
        SetIsActive(true);
        SetIsFailed(false);

    }
    public override void UpdateObjective()
    {
        if (GetIsCompleted() || GetIsFailed()) return;

        if (!escortShip.activeSelf) FailedObjective();

        // Move the ship based on the current and next checkpoint
        if (currentCheckpoints >= 0 && currentCheckpoints < requiredCheckpoints)
        {
            int nextCheckpointIndex = currentCheckpoints + 1;
            Vector3 targetAim = escortPathways[nextCheckpointIndex];
            Vector3 direction = targetAim - escortShip.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            escortShip.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 270f));
            escortShip.transform.position = Vector3.MoveTowards(
                escortShip.transform.position,
                escortPathways[nextCheckpointIndex],
                distancePerCheckpoint * Time.deltaTime
            );
            if (Vector3.Distance(escortShip.transform.position, escortPathways[nextCheckpointIndex]) < 1f)
            {
                currentCheckpoints++;
                if (currentCheckpoints == requiredCheckpoints)
                {
                    CompleteObjective();
                    GameManager.Instance.RemoveEnemy(escortShip, escortShip.GetComponent<Faction>());
                    escortShip.GetComponent<CargoShip>().TeleportAway();
                }
            }
        }
        if (GetIsActive() && !GetIsCompleted() && !GetIsFailed()) SetObjectiveDescription($"Escort the ship to the next checkpoint" + $"\nCheckpoints remaining: {requiredCheckpoints - currentCheckpoints}");

        if (GetIsCompleted()) SetObjectiveDescription("Objective Completed");

        if (GetIsFailed()) SetObjectiveDescription("Objective Failed");


    }
    public override void CompleteObjective()
    {

        MarkObjectiveCompleted();
    }

    public override void FailedObjective()
    {
        MarkObjectiveFailed();
    }


}
