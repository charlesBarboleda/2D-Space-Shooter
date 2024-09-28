using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LevelType
{
    Horde,
    Boss,
    Survival,
    Invasion,
    Protect,
    Rescue,
}
public abstract class Level
{
    protected LevelType _leveltype;
    protected FactionType _factionType = FactionType.Player;
    protected List<Ship> _shipsToSpawn = new List<Ship>();
    protected Dictionary<string, GameObject> shipList = new Dictionary<string, GameObject>();

    public abstract void StartLevel();
    public abstract void UpdateLevel();
    public abstract void CompleteLevel();

}
