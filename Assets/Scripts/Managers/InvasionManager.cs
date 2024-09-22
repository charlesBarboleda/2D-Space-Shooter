using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InvasionManager : MonoBehaviour
{
    public static InvasionManager Instance { get; private set; }
    [SerializeField] FactionType _defendingFaction;
    [SerializeField]
    Dictionary<FactionType, int> _factionInvasionProgress = new Dictionary<FactionType, int>
{
    { FactionType.Syndicates, 0 },
    { FactionType.CrimsonFleet, 0 },
    { FactionType.ThraxArmada, 0 }
};
    [SerializeField] List<FactionType> _invadingFactions = new List<FactionType>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        _invadingFactions.Add(FactionType.CrimsonFleet);
        _defendingFaction = FactionType.ThraxArmada;
    }


    void OnEnable()
    {
        EventManager.OnFactionInvasionWon += OnInvasionWon;


    }

    void OnDisable()
    {
        EventManager.OnFactionInvasionWon -= OnInvasionWon;

    }

    void OnInvasionWon(FactionType faction)
    {
        _factionInvasionProgress[faction]++;
        if (_factionInvasionProgress[faction] >= 3)
        {
            _invadingFactions.Remove(faction);
            _defendingFaction = faction;
            _factionInvasionProgress[faction] = 0;
        }
    }


    public FactionType DefendingFaction { get => _defendingFaction; }
    public List<FactionType> InvadingFactions { get => _invadingFactions; }

}
