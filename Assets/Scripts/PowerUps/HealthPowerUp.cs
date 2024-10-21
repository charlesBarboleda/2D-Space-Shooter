using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : PowerUp, IPickable
{
    bool _isAttracted;
    public bool isAttracted { get => _isAttracted; set => _isAttracted = value; }
    float _maxSpeed;
    public float maxSpeed { get => _maxSpeed; set => _maxSpeed = value; }
    float _initHealth;
    float _initMaxHealth;
    void FixedUpdate()
    {

        if (isAttracted)
        {
            MoveTowardsPlayer();
        }
    }

    public void OnPickUp()
    {
        Effect();
        gameObject.SetActive(false);
    }


    public void MoveTowardsPlayer()
    {
        Vector2 playerPosition = PlayerManager.GetInstance().transform.position;
        transform.position = Vector2.MoveTowards(transform.position, playerPosition, _maxSpeed * Time.deltaTime);
    }

    protected override void Effect()
    {

        PlayerManager.Instance.SetCurrentHealth(PlayerManager.Instance.CurrentHealth() * 2);
        PlayerManager.Instance.SetMaxHealth(PlayerManager.Instance.MaxHealth() * 2);
        UIManager.Instance.HealthPowerUp.SetActive(true);

        UIManager.Instance.MidScreenWarningText("Health power up!", 1f);
    }

    public override void DeactivateEffect()
    {
        Debug.Log("Health PowerUp Deactivated");
        _initHealth = PlayerManager.Instance.CurrentHealth() / 2;
        _initMaxHealth = PlayerManager.Instance.MaxHealth() / 2;
        PlayerManager.Instance.SetCurrentHealth(_initHealth);
        PlayerManager.Instance.SetMaxHealth(_initMaxHealth);
        UIManager.Instance.HealthPowerUp.SetActive(false);


        Debug.Log("Health: " + PlayerManager.Instance.CurrentHealth());
    }


}
