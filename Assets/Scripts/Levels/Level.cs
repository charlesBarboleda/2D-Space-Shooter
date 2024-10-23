using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LevelType
{
    Horde,
    Boss,
    MultiPhaseBoss,
    Invasion,
    Comet
}
public abstract class Level
{
    protected LevelType _leveltype;
    protected FactionType _factionType = FactionType.Player;
    protected List<ObjectiveBase> _levelObjectives = new List<ObjectiveBase>();
    protected List<Ship> _shipsToSpawn = new List<Ship>();
    protected List<GameObject> shipList = new List<GameObject>();
    public abstract void StartLevel();
    public abstract void UpdateLevel();
    public abstract void CompleteLevel();
    public virtual List<ObjectiveBase> GetLevelObjectives()
    {
        return _levelObjectives;
    }

}
