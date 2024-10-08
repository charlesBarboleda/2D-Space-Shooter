using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance { get; private set; }
    public int comboCount = 0;
    public float comboTimer = 5f;
    PlayerManager _playerManager;


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
        _playerManager = PlayerManager.Instance;
    }

    void Update()
    {
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
        }
        else
        {
            ResetCombo();
        }
    }

    public void IncreaseCombo()
    {
        comboCount++;
        comboTimer = 5f;
    }

    public void ResetCombo()
    {
        comboCount = 0;
        comboTimer = 5f;
        _playerManager.ComboManager().RemoveAllBuffs();
        UIManager.Instance.DeactivateComboKey();
    }
}
