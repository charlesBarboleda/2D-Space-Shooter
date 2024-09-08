using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour
{
    [SerializeField] FactionType _factionType;
    List<Faction> _enemyFactions = new List<Faction>();
    List<Faction> _allyFactions = new List<Faction>();
    public FactionType factionType { get => _factionType; set => _factionType = value; }

}
