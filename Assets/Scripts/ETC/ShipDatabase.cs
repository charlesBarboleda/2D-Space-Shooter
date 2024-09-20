using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShipDatabase
{
    public static List<Ship> CrimsonFleetEarly = new List<Ship> {
        new Ship { name = "CrimsonSmall1", weight = 0.25f },
        new Ship { name = "CrimsonSmall2", weight = 0.25f },
        new Ship { name = "CrimsonSmall3", weight = 0.25f },
        new Ship { name = "CrimsonSmall4", weight = 0.25f },
    };

    public static List<Ship> CrimsonFleetMid = new List<Ship> {
        new Ship { name = "CrimsonSmall1", weight = 0.15f },
        new Ship { name = "CrimsonSmall2", weight = 0.15f },
        new Ship { name = "CrimsonSmall3", weight = 0.15f },
        new Ship { name = "CrimsonSmall4", weight = 0.15f },
        new Ship { name = "CrimsonBomber", weight = 0.38f },
        new Ship { name = "CrimsonBuffer", weight = 0.02f }
    };

    public static List<Ship> CrimsonFleetLate = new List<Ship> {
        new Ship { name = "CrimsonSmall1", weight = 0.15f },
        new Ship { name = "CrimsonSmall2", weight = 0.15f },
        new Ship { name = "CrimsonSmall3", weight = 0.15f },
        new Ship { name = "CrimsonSmall4", weight = 0.15f },
        new Ship { name = "CrimsonBomber", weight = 0.38f },
        new Ship { name = "CrimsonBomberSpawner", weight = 0.01f },
        new Ship { name = "CrimsonBuffer", weight = 0.01f }
    };
    public static List<Ship> ThraxArmadaEarly = new List<Ship> {
        new Ship { name = "ThraxSmall1", weight = 0.333f },
        new Ship { name = "ThraxSmall2", weight = 0.333f },
        new Ship { name = "ThraxSmall3", weight = 0.333f },
    };
    public static List<Ship> ThraxArmadaMid = new List<Ship> {
        new Ship { name = "ThraxSmall1", weight = 0.3f },
        new Ship { name = "ThraxSmall2", weight = 0.3f },
        new Ship { name = "ThraxSmall3", weight = 0.3f },
        new Ship { name = "ThraxTeleporter1", weight = 0.05f },
        new Ship { name = "ThraxTeleporter2", weight = 0.05f },
    };

    public static List<Ship> ThraxArmadaLate = new List<Ship> {
        new Ship { name = "ThraxSmall1", weight = 0.298f },
        new Ship { name = "ThraxSmall2", weight = 0.298f },
        new Ship { name = "ThraxSmall3", weight = 0.298f },
        new Ship { name = "ThraxTeleporter1", weight = 0.05f },
        new Ship { name = "ThraxTeleporter2", weight = 0.05f },
        new Ship { name = "ThraxCarrier1", weight = 0.005f },
        new Ship { name = "ThraxBoss1", weight = 0.001f }
    };

    public static List<Ship> SyndicatesEarly = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.5f },
        new Ship { name = "MeleeShip", weight = 0.5f }
    };
    public static List<Ship> SyndicatesEarly2 = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.3f },
        new Ship { name = "MeleeShip", weight = 0.3f },
        new Ship { name = "MediumShip", weight = 0.2f },
        new Ship { name = "MediumShip2", weight = 0.2f },
    };
    public static List<Ship> SyndicatesEarly3 = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.1f },
        new Ship { name = "MeleeShip", weight = 0.1f },
        new Ship { name = "MediumShip", weight = 0.4f },
        new Ship { name = "MediumShip2", weight = 0.4f },
    };
    public static List<Ship> SyndicatesMid = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.1f },
        new Ship { name = "MeleeShip", weight = 0.1f },
        new Ship { name = "MediumShip", weight = 0.35f },
        new Ship { name = "MediumShip2", weight = 0.4f },
        new Ship { name = "LargeShip", weight = 0.05f },

    };
    public static List<Ship> SyndicatesMid2 = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.05f },
        new Ship { name = "MeleeShip", weight = 0.05f },
        new Ship { name = "MediumShip", weight = 0.3f },
        new Ship { name = "MediumShip2", weight = 0.4f },
        new Ship { name = "LargeShip", weight = 0.2f },
    };
    public static List<Ship> SyndicatesMid3 = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.05f },
        new Ship { name = "MeleeShip", weight = 0.05f },
        new Ship { name = "MediumShip", weight = 0.3f },
        new Ship { name = "MediumShip2", weight = 0.35f },
        new Ship { name = "LargeShip", weight = 0.2f },
        new Ship { name = "NukeShip", weight = 0.05f },
    };
    public static List<Ship> SyndicateLate = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.05f },
        new Ship { name = "MeleeShip", weight = 0.05f },
        new Ship { name = "MediumShip", weight = 0.3f },
        new Ship { name = "MediumShip2", weight = 0.35f },
        new Ship { name = "LargeShip", weight = 0.2f },
        new Ship { name = "NukeShip", weight = 0.05f },
    };
    public static List<Ship> SyndicatesLate2 = new List<Ship> {
        new Ship { name = "SmallShip", weight = 0.05f },
        new Ship { name = "MeleeShip", weight = 0.05f },
        new Ship { name = "MediumShip", weight = 0.3f },
        new Ship { name = "MediumShip2", weight = 0.3f },
        new Ship { name = "LargeShip", weight = 0.2f },
        new Ship { name = "NukeShip", weight = 0.05f },
        new Ship { name = "NukeShip2", weight = 0.05f },


    };
    public static List<Ship> SyndicatesLate3 = new List<Ship> {
        new Ship { name = "MediumShip", weight = 0.2f },
        new Ship { name = "MediumShip2", weight = 0.2f },
        new Ship { name = "LargeShip", weight = 0.3f },
        new Ship { name = "NukeShip2", weight = 0.3f },

    };
}
