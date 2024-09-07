using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour
{
    List<PowerUp> _powerUps = new List<PowerUp>();

    // Update is called once per frame
    void Update()
    {
        if (_powerUps.Count > 0)
        {
            foreach (PowerUp powerUp in _powerUps)
            {
                powerUp.duration -= Time.deltaTime;
                if (powerUp.duration <= 0)
                {
                    powerUp.duration = 0;
                    powerUp.DeactivateEffect();
                    _powerUps.Remove(powerUp);
                }
            }
        }
    }

    public void AddPowerUp(PowerUp powerUp)
    {
        _powerUps.Add(powerUp);
    }
}
