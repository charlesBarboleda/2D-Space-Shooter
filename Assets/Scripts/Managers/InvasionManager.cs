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
        // *Trial version code*
        _defendingFaction = FactionType.Syndicates;
        _invadingFactions.Add(FactionType.ThraxArmada);
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

            // Add a new random faction to the invasion list excluding the defending faction and the player faction
            FactionType _randomFaction = (FactionType)Random.Range(0, 4);
            while (_randomFaction == _defendingFaction || _randomFaction == FactionType.Player)
            {
                _randomFaction = (FactionType)Random.Range(0, 4);
            }
            _invadingFactions.Add(_randomFaction);




        }
    }


    public FactionType DefendingFaction { get => _defendingFaction; }
    public List<FactionType> InvadingFactions { get => _invadingFactions; }

}
