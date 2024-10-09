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
        // *Trial version code*
        _defendingFaction = FactionType.ThraxArmada;
        _invadingFactions.Add(FactionType.ThraxArmada);

        // *Real version code*
        // _defendingFaction = FactionType.Syndicates;
        // _invadingFactions.Add(FactionType.CrimsonFleet);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {

            Debug.Log($"Thrax Invasion Progress: {_factionInvasionProgress[FactionType.ThraxArmada]} / 3");
            Debug.Log($"Syndicates Invasion Progress: {_factionInvasionProgress[FactionType.Syndicates]} / 3");
            Debug.Log($"Crimson Fleet Invasion Progress: {_factionInvasionProgress[FactionType.CrimsonFleet]} / 3");
            Debug.Log($"Defending Faction: {_defendingFaction}");
            Debug.Log($"Invading Factions: {string.Join(", ", _invadingFactions)}");
        }
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
