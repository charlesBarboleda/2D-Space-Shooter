using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class PowerUpsManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;

    [SerializeField] private PlayerController playerController;

    private List<(System.Action action, string name)> shipSpeedFunctions = new List<(System.Action, string)>();
    private List<(System.Action action, string name)> healthFunctions = new List<(System.Action, string)>();
    private List<(System.Action action, string name)> turretFunctions = new List<(System.Action, string)>();
    private List<(System.Action action, string name)> shieldFunctions = new List<(System.Action, string)>();
    private List<(System.Action action, string name)> laserFunctions = new List<(System.Action, string)>();
    private List<(System.Action action, string name)> bulletDamageFunctions = new List<(System.Action, string)>();
    private List<(System.Action action, string name)> bulletSpeedFunctions = new List<(System.Action, string)>();
    private List<(System.Action action, string name)> extraBulletFunctions = new List<(System.Action, string)>();
    private List<(System.Action action, string name)> fireRateFunctions = new List<(System.Action, string)>();

    public Button[] buttons;

    private void OnEnable()
    {
        InitPowerUpButtons();
    }
    private void InitPowerUpButtons()
    {
        ClearFunctionLists();
        ChangeButtonColor(Color.white);

        // Initialize the appropriate function lists based on GameManager level
        if (GameManager.Instance.level % 50 == 0)
        {
            InitializeBossFunctions();
            ChangeButtonColor(Color.red);
        }
        else if (GameManager.Instance.level >= 40)
        {
            InitializeLevel3Functions();

        }
        else if (GameManager.Instance.level >= 20)
        {
            InitializeLevel2Functions();

        }
        else if (GameManager.Instance.level >= 1)
        {
            InitializeLevel1Functions();

        }

        AssignFunctionsToButtons();
    }
    private void InitializeBossFunctions()
    {

        shipSpeedFunctions.AddRange(new List<(System.Action, string)>
    {
        (ShipSpeedX, nameof(ShipSpeedX)),

    });
        healthFunctions.AddRange(new List<(System.Action, string)>
    {
        (HealthX, nameof(HealthX)),
    });
        turretFunctions.AddRange(new List<(System.Action, string)>
    {
        (TurretX, nameof(TurretX)),
    });
        shieldFunctions.AddRange(new List<(System.Action, string)>
    {
        (ShieldX, nameof(ShieldX)),
    });
        laserFunctions.AddRange(new List<(System.Action, string)>
    {
        (LaserX, nameof(LaserX)),
    });
        bulletDamageFunctions.AddRange(new List<(System.Action, string)>
    {
        (BulletDamageX, nameof(BulletDamageX)),
    });
        bulletSpeedFunctions.AddRange(new List<(System.Action, string)>
    {
        (BulletSpeedX, nameof(BulletSpeedX)),
    });
        fireRateFunctions.AddRange(new List<(System.Action, string)>
    {
        (FireRateX, nameof(FireRateX)),
    });
        extraBulletFunctions.AddRange(new List<(System.Action, string)>
    {
        (ExtraBulletX, nameof(ExtraBulletX)),
    });
    }

    private void ClearFunctionLists()
    {
        shipSpeedFunctions.Clear();
        healthFunctions.Clear();
        turretFunctions.Clear();
        shieldFunctions.Clear();
        laserFunctions.Clear();
        bulletDamageFunctions.Clear();
        bulletSpeedFunctions.Clear();
        extraBulletFunctions.Clear();
        fireRateFunctions.Clear();
    }

    private void InitializeLevel3Functions()
    {
        shipSpeedFunctions.AddRange(new List<(System.Action, string)>
    {
        (ShipSpeedI, nameof(ShipSpeedI)),
        (ShipSpeedII, nameof(ShipSpeedII)),
        (ShipSpeedIII, nameof(ShipSpeedIII)),
    });
        healthFunctions.AddRange(new List<(System.Action, string)>
    {
        (HealthI, nameof(HealthI)),
        (HealthII, nameof(HealthII)),
        (HealthIII, nameof(HealthIII)),
    });
        turretFunctions.AddRange(new List<(System.Action, string)>
    {
        (TurretI, nameof(TurretI)),
        (TurretII, nameof(TurretII)),
        (TurretIII, nameof(TurretIII)),

    });
        shieldFunctions.AddRange(new List<(System.Action, string)>
    {
        (ShieldI, nameof(ShieldI)),
        (ShieldII, nameof(ShieldII)),
        (ShieldIII, nameof(ShieldIII)),
    });
        laserFunctions.AddRange(new List<(System.Action, string)>
    {
        (LaserI, nameof(LaserI)),
        (LaserII, nameof(LaserII)),
        (LaserIII, nameof(LaserIII)),
    });
        bulletDamageFunctions.AddRange(new List<(System.Action, string)>
    {
        (BulletDamageI, nameof(BulletDamageI)),
        (BulletDamageII, nameof(BulletDamageII)),
        (BulletDamageIII, nameof(BulletDamageIII)),
    });
        bulletSpeedFunctions.AddRange(new List<(System.Action, string)>
    {
        (BulletSpeedI, nameof(BulletSpeedI)),
        (BulletSpeedII, nameof(BulletSpeedII)),
        (BulletSpeedIII, nameof(BulletSpeedIII)),
    });
        fireRateFunctions.AddRange(new List<(System.Action, string)>
    {
        (FireRateI, nameof(FireRateI)),
        (FireRateII, nameof(FireRateII)),
        (FireRateIII, nameof(FireRateIII)),
    });
        extraBulletFunctions.AddRange(new List<(System.Action, string)>
    {
        (ExtraBullet, nameof(ExtraBullet)),
    });
    }

    private void InitializeLevel2Functions()
    {
        shipSpeedFunctions.AddRange(new List<(System.Action, string)>
    {
        (ShipSpeedI, nameof(ShipSpeedI)),
        (ShipSpeedII, nameof(ShipSpeedII)),
    });
        healthFunctions.AddRange(new List<(System.Action, string)>
    {
        (HealthI, nameof(HealthI)),
        (HealthII, nameof(HealthII)),
    });
        turretFunctions.AddRange(new List<(System.Action, string)>
    {
        (TurretI, nameof(TurretI)),
        (TurretII, nameof(TurretII)),
    });
        shieldFunctions.AddRange(new List<(System.Action, string)>
    {
        (ShieldI, nameof(ShieldI)),
        (ShieldII, nameof(ShieldII)),
    });
        laserFunctions.AddRange(new List<(System.Action, string)>
    {
        (LaserI, nameof(LaserI)),
        (LaserII, nameof(LaserII)),
    });
        bulletDamageFunctions.AddRange(new List<(System.Action, string)>
    {
        (BulletDamageI, nameof(BulletDamageI)),
        (BulletDamageII, nameof(BulletDamageII)),
    });
        bulletSpeedFunctions.AddRange(new List<(System.Action, string)>
    {
        (BulletSpeedI, nameof(BulletSpeedI)),
        (BulletSpeedII, nameof(BulletSpeedII)),
    });
        fireRateFunctions.AddRange(new List<(System.Action, string)>
    {
        (FireRateI, nameof(FireRateI)),
        (FireRateII, nameof(FireRateII)),
    });
    }

    private void InitializeLevel1Functions()
    {
        shipSpeedFunctions.AddRange(new List<(System.Action, string)>
    {
        (ShipSpeedI, nameof(ShipSpeedI)),
    });
        healthFunctions.AddRange(new List<(System.Action, string)>
    {
        (HealthI, nameof(HealthI)),
    });
        turretFunctions.AddRange(new List<(System.Action, string)>
    {
        (TurretI, nameof(TurretI)),
    });
        shieldFunctions.AddRange(new List<(System.Action, string)>
    {
        (ShieldI, nameof(ShieldI)),
    });
        laserFunctions.AddRange(new List<(System.Action, string)>
    {
        (LaserI, nameof(LaserI)),
    });
        bulletDamageFunctions.AddRange(new List<(System.Action, string)>
    {
        (BulletDamageI, nameof(BulletDamageI)),
    });
        bulletSpeedFunctions.AddRange(new List<(System.Action, string)>
    {
        (BulletSpeedI, nameof(BulletSpeedI)),
    });
        fireRateFunctions.AddRange(new List<(System.Action, string)>
    {
        (FireRateI, nameof(FireRateI)),
    });


    }
    private void AssignFunctionsToButtons()
    {
        // Create a list of all function lists
        List<List<(System.Action action, string name)>> functionGroups = new List<List<(System.Action, string)>>
    {
        shipSpeedFunctions,
        healthFunctions,
        turretFunctions,
        shieldFunctions,
        laserFunctions,
        bulletDamageFunctions,
        bulletSpeedFunctions,
        fireRateFunctions
    };

        // Only add extraBulletFunctions if level is 20 or above
        if (GameManager.Instance.level >= 50)
        {
            functionGroups.Add(extraBulletFunctions);
        }

        // Create a list to store the selected functions
        List<(System.Action action, string name)> selectedFunctions = new List<(System.Action, string)>();

        // Select a random function from each group
        foreach (var functionGroup in functionGroups)
        {
            if (functionGroup.Count > 0)
            {
                // Select a random index from the function group
                int randomIndex = UnityEngine.Random.Range(0, functionGroup.Count);

                // Add the selected function to the selected functions list
                selectedFunctions.Add(functionGroup[randomIndex]);
            }
        }

        // Assign the selected functions to the buttons and set the button text
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < selectedFunctions.Count)
            {
                int randomIndex = UnityEngine.Random.Range(0, selectedFunctions.Count);
                var selectedFunction = selectedFunctions[randomIndex];
                selectedFunctions.RemoveAt(randomIndex);
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].onClick.AddListener(() => selectedFunction.action());

                // Get the Text or TextMeshPro component and set the button text
                Text textComponent = buttons[i].GetComponentInChildren<Text>();
                if (textComponent != null)
                {
                    textComponent.text = AddSpacesToName(selectedFunction.name);
                }
                else
                {
                    TextMeshProUGUI tmpComponent = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
                    if (tmpComponent != null)
                    {
                        tmpComponent.text = AddSpacesToName(selectedFunction.name);
                    }
                }
            }
        }
    }


    private string AddSpacesToName(string name)
    {
        return Regex.Replace(name, "(\\B[A-Z])", " $1");
    }

    private void FireRateI()
    {
        playerManager.weapon.ReduceFireRate(0.01f);
        Debug.Log("FireRateI");
        changeState();

    }
    private void FireRateII()
    {
        playerManager.weapon.ReduceFireRate(0.015f);
        Debug.Log("FireRateII");
        changeState();

    }
    private void FireRateIII()
    {
        playerManager.weapon.ReduceFireRate(0.02f);
        Debug.Log("FireRateIII");
        changeState();

    }
    private void FireRateX()
    {
        playerManager.weapon.ReduceFireRate(0.06f);
        Debug.Log("FireRateX");
        changeState();

    }

    private void ShipSpeedI()
    {
        playerController.playerSpeed += 1f;
        Debug.Log("ShipSpeedI");
        changeState();

    }
    private void ShipSpeedII()
    {
        playerController.playerSpeed += 1.5f;
        Debug.Log("ShipSpeedII");
        changeState();
    }
    private void ShipSpeedIII()
    {
        playerController.playerSpeed += 2f;
        Debug.Log("ShipSpeedIII");
        changeState();

    }
    private void ShipSpeedX()
    {
        playerController.playerSpeed += 4f;
        Debug.Log("ShipSpeedX");
        changeState();

    }

    private void HealthI()
    {
        playerManager.AddHealth(100f);
        Debug.Log("HealthI");
        changeState();
    }
    private void HealthII()
    {
        playerManager.AddHealth(200f);
        Debug.Log("HealthII");
        changeState();

    }
    private void HealthIII()
    {
        playerManager.AddHealth(400f);
        Debug.Log("HealthIII");
        changeState();
    }
    private void HealthX()
    {
        playerManager.AddHealth(800f);
        Debug.Log("HealthX");
        changeState();
    }


    private void ExtraBullet()
    {
        playerManager.weapon.IncreaseBullets(1);
        Debug.Log("ExtraExtraBulletI");
        changeState();
    }
    private void ExtraBulletX()
    {
        playerManager.weapon.IncreaseBullets(3);
        Debug.Log("ExtraBulletX");
        changeState();
    }

    private void TurretI()
    {
        playerManager.turret.AddTurretTime(1f);
        Debug.Log("TurretI");
        changeState();
    }
    private void TurretII()
    {
        playerManager.turret.AddTurretTime(2f);
        Debug.Log("TurretII");
        changeState();
    }
    private void TurretIII()
    {
        playerManager.turret.AddTurretTime(4f);
        Debug.Log("TurretIII");
        changeState();
    }
    private void TurretX()
    {
        playerManager.turret.AddTurretTime(8f);
        Debug.Log("TurretX");
        changeState();
    }

    private void ShieldI()
    {
        playerManager.playerShield.AddShieldTime(15f);
        Debug.Log("ShieldI");
        changeState();
    }
    private void ShieldII()
    {
        playerManager.playerShield.AddShieldTime(30f);
        Debug.Log("ShieldII");
        changeState();

    }
    private void ShieldIII()
    {
        playerManager.playerShield.AddShieldTime(60f);
        Debug.Log("ShieldIII");
        changeState();
    }
    private void ShieldX()
    {
        playerManager.playerShield.AddShieldTime(120f);
        Debug.Log("ShieldX");
        changeState();
    }

    private void LaserI()
    {
        playerManager.laser.IncreaseStock(1);
        Debug.Log("LaserStock");
        changeState();
    }

    private void LaserII()
    {
        playerManager.laser.IncreaseStock(2);
        Debug.Log("LaserStockII");
        changeState();

    }
    private void LaserIII()
    {
        playerManager.laser.IncreaseStock(3);
        Debug.Log("LaserStockIII");
        changeState();
    }
    private void LaserX()
    {
        playerManager.laser.IncreaseStock(4);
        Debug.Log("LaserStockX");
        changeState();
    }

    private void BulletDamageI()
    {
        playerManager.weapon.IncreaseBulletDamage(40);
        Debug.Log("BulletDamage");
        changeState();

    }
    private void BulletDamageII()
    {
        playerManager.weapon.IncreaseBulletDamage(80);
        Debug.Log("BulletDamageII");
        changeState();

    }
    private void BulletDamageIII()
    {
        playerManager.weapon.IncreaseBulletDamage(150);
        Debug.Log("BulletDamageIII");
        changeState();
    }

    private void BulletDamageX()
    {
        playerManager.weapon.IncreaseBulletDamage(300);
        Debug.Log("BulletDamageX");
        changeState();
    }
    private void BulletSpeedI()
    {
        playerManager.weapon.IncreaseBulletSpeed(2);
        Debug.Log("BulletSpeedI");
        changeState();

    }
    private void BulletSpeedII()
    {
        playerManager.weapon.IncreaseBulletSpeed(4);
        Debug.Log("BulletSpeedII");
        changeState();

    }
    private void BulletSpeedIII()
    {
        playerManager.weapon.IncreaseBulletSpeed(6);
        Debug.Log("BulletSpeedIII");
        changeState();
    }
    private void BulletSpeedX()
    {
        playerManager.weapon.IncreaseBulletSpeed(10);
        Debug.Log("BulletSpeedX");
        changeState();
    }

    private void changeState()
    {

        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void ChangeButtonColor(Color color)
    {
        foreach (var button in buttons)
        {
            button.GetComponent<Image>().color = color;
        }
    }

}
