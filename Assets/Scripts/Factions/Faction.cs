using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FactionType
{
    Player,
    CrimsonFleet,
    ThraxArmada,
    Syndicates,
}

public class Faction : MonoBehaviour
{
    [SerializeField] FactionType _factionType;
    public FactionType factionType { get => _factionType; set => _factionType = value; }
}
