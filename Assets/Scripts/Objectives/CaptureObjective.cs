using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(fileName = "CaptureObjective", menuName = "Objectives/CaptureObjective")]
public class CaptureObjective : ObjectiveBase
{
    public float captureTime = 30f;
    public float captureProgress = 0f;
    float elapsedTime = 0f;
    public float timeToCapture = 180f;
    Vector3 capturePosition;
    GameObject captureCirclePrefab;
    GameObject greenCaptureCircle;
    CaptureCircle captureCircle;

    public override void Initialize()
    {
        elapsedTime = timeToCapture;

        capturePosition = SpawnerManager.Instance.GetRandomPositionOutsideBuildings();
        captureCirclePrefab = ObjectPooler.Instance.SpawnFromPool("CaptureCircle", capturePosition, Quaternion.identity);
        greenCaptureCircle = ObjectPooler.Instance.SpawnFromPool("GreenCaptureCircle", capturePosition, Quaternion.identity);
        SpawnerManager.Instance.EnemiesList.Add(captureCirclePrefab);

        rewardPoints = LevelManager.Instance.CurrentLevelIndex * 200;

        captureCircle = greenCaptureCircle.GetComponent<CaptureCircle>();
        captureCircle.captureTime = captureTime;
        captureProgress = 0f;
    }
    public override void UpdateObjective()
    {
        if (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            if (captureCircle.IsCaptured())
            {
                CompleteObjective();
                captureCirclePrefab.SetActive(false);
                greenCaptureCircle.SetActive(false);
            }
            if (SpawnerManager.Instance.EnemiesToSpawnLeft <= 0 && SpawnerManager.Instance.EnemiesList.Count == 1)
            {
                CompleteObjective();
            }
            objectiveDescription = "Capture the Magic Circle: " + Mathf.Round(captureCircle.progress / captureCircle.captureTime * 100).ToString() + "%" + " in " + Mathf.Round(elapsedTime) + " seconds";
            ObjectiveManager.Instance.UpdateObjectivesUI();
        }
        else
        {
            FailObjective();
            captureCirclePrefab.SetActive(false);
            greenCaptureCircle.SetActive(false);
        }

    }
    public override void CompleteObjective()
    {
        // Notify ObjectiveManager of completion
        objectiveDescription = "The Magic Circle has been captured";
        // Force UI to update
        ObjectiveManager.Instance.UpdateObjectivesUI();
        base.CompleteObjective();
        ObjectiveManager.Instance.HandleObjectiveCompletion(this);
        isObjectiveCompleted = true;
        SpawnerManager.Instance.EnemiesList.Remove(captureCirclePrefab);


        Debug.Log("Completed Objective from Invasion Objective");

    }
    public override void FailObjective()
    {
        isObjectiveFailed = true;
        SpawnerManager.Instance.EnemiesList.Remove(captureCirclePrefab);
        objectiveDescription = "The Magic Circle has run out!";
        ObjectiveManager.Instance.UpdateObjectivesUI();
        ObjectiveManager.Instance.HandleObjectiveFailure(this);
        Debug.Log("Failed Objective from Invasion Objective");
    }

}
