using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscortObjective : Objective
{
    [SerializeField] int requiredCheckpoints;
    [SerializeField] List<Vector3> escortPathway;
    [SerializeField] float distancePerCheckpoint;
    [SerializeField] GameObject shipPrefab;
    GameObject escortShip;
    public override void InitObjective()
    {
        requiredCheckpoints = 4;
        escortShip = Instantiate(shipPrefab, escortPathway[0], Quaternion.identity);
        SetIsCompleted(false);
        SetIsActive(true);
        SetIsFailed(false);

    }
    public override void UpdateObjective()
    {
        if (GetIsCompleted()) return;

        if (Vector3.Distance(escortShip.transform.position, escortPathway[requiredCheckpoints]) < distancePerCheckpoint)
        {
            requiredCheckpoints--;
        }

        if (requiredCheckpoints == 0)
        {
            CompleteObjective();
        }

    }
    public override void CompleteObjective()
    {

    }

    public override void FailedObjective()
    {

    }

}
