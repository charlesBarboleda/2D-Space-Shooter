using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGasExplosionFullMap", menuName = "EnemyAbilities/EnemyGasExplosionFullMap")]
public class EnemyGasExplosionFullMapAbility : Ability
{
    public enum AbilityDirection
    {
        Vertical,
        Horizontal,
        Plus,
        Cross,
        DiagonalLeft,
        DiagonalRight
    }
    public AbilityDirection abilityDirection = AbilityDirection.Vertical;
    [SerializeField] List<Transform> _verticalGasExplosionPositions = new List<Transform>();
    [SerializeField] List<Transform> _horizontalGasExplosionPositions = new List<Transform>();
    [SerializeField] List<Transform> _plusGasExplosionPositions = new List<Transform>();
    [SerializeField] List<Transform> _crossGasExplosionPositions = new List<Transform>();
    [SerializeField] List<Transform> _diagonalLeftGasExplosionPositions = new List<Transform>();
    [SerializeField] List<Transform> _diagonalRightGasExplosionPositions = new List<Transform>();

    GameObject _gasExplosion;

    [SerializeField] string _gasExplosionTag = "GasExplosionLarge";

    public override void AbilityLogic(GameObject owner, Transform target)
    {

        switch (abilityDirection)
        {
            case AbilityDirection.Vertical:
                SpawnGasExplosionVertical();
                break;
            case AbilityDirection.Horizontal:
                SpawnGasExplosionHorizontal();
                break;
            case AbilityDirection.Plus:
                SpawnGasExplosionPlus();
                break;
            case AbilityDirection.Cross:
                SpawnGasExplosionCross();
                break;
            case AbilityDirection.DiagonalLeft:
                SpawnGasExplosionDiagonalLeft();
                break;
            case AbilityDirection.DiagonalRight:
                SpawnGasExplosionDiagonalRight();
                break;
        }
    }

    void SpawnGasExplosionDiagonalLeft()
    {
        foreach (Transform position in _diagonalLeftGasExplosionPositions)
        {
            SpawnGasExplosion(position.position);
        }
    }
    void SpawnGasExplosionDiagonalRight()
    {
        foreach (Transform position in _diagonalRightGasExplosionPositions)
        {
            SpawnGasExplosion(position.position);
        }
    }

    void SpawnGasExplosionVertical()
    {
        foreach (Transform position in _verticalGasExplosionPositions)
        {
            SpawnGasExplosion(position.position);
        }
    }
    void SpawnGasExplosionHorizontal()
    {
        foreach (Transform position in _horizontalGasExplosionPositions)
        {
            SpawnGasExplosion(position.position);
        }

    }
    void SpawnGasExplosionPlus()
    {
        foreach (Transform position in _plusGasExplosionPositions)
        {
            SpawnGasExplosion(position.position);
        }
    }
    void SpawnGasExplosionCross()
    {
        foreach (Transform position in _crossGasExplosionPositions)
        {
            SpawnGasExplosion(position.position);
        }
    }

    void SpawnGasExplosion(Vector3 position)
    {
        _gasExplosion = ObjectPooler.Instance.SpawnFromPool(_gasExplosionTag, position, Quaternion.identity);
    }

    public override void ResetStats()
    {
        cooldown = 120f;
        currentCooldown = 120f;
        isUnlocked = false;
    }



}
