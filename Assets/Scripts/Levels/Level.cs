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
    protected List<Objective> _objectives = new List<Objective>();
    protected List<Ship> _shipsToSpawn = new List<Ship>();
    protected int _levelNumber;

    public abstract void StartLevel();
    public abstract void UpdateLevel();
    public abstract void CompleteLevel();

}
