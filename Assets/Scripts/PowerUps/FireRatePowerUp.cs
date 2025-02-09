using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRatePowerUp : PowerUp, IPickable
{
    bool _isAttracted;
    public bool isAttracted { get => _isAttracted; set => _isAttracted = value; }
    float _maxSpeed;
    public float maxSpeed { get => _maxSpeed; set => _maxSpeed = value; }
    float _initFireRate;
    Weapon _weapon;
    void Start()
    {
        _weapon = PlayerManager.Instance.Weapon();
    }
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
        UIManager.Instance.FireRatePowerUp.SetActive(true);
        UIManager.Instance.MidScreenWarningText("Fire Rate power up!", 1f);

        _weapon.fireRate /= 2;
        Debug.Log("FireRate: " + _weapon.fireRate);
    }

    public override void DeactivateEffect()
    {
        Debug.Log("FireRate PowerUp Deactivated");
        _initFireRate = _weapon.fireRate * 2;
        _weapon.fireRate = _initFireRate;
        UIManager.Instance.FireRatePowerUp.SetActive(false);
        Debug.Log("FireRate: " + _weapon.fireRate);
    }


}
