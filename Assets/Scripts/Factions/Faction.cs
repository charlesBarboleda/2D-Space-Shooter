using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour
{
    [SerializeField] FactionType _factionType;
    [SerializeField] List<FactionType> _allyFactions = new List<FactionType>();


    public void AddAllyFaction(FactionType faction)
    {
        if (!_allyFactions.Contains(faction)) _allyFactions.Add(faction);

    }

    public void RemoveAllyFaction(FactionType faction)
    {
        if (_allyFactions.Contains(faction)) _allyFactions.Remove(faction);

    }

    public bool IsHostileTo(FactionType FactionType)
    {
        // Return true only if the factions are different
        if (_allyFactions.Contains(FactionType)) return false;
        else return true;
    }

    public FactionType factionType { get => _factionType; set => _factionType = value; }
}
