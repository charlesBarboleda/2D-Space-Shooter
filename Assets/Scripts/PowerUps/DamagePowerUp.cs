using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePowerUp : PowerUp, IPickable
{
    bool _isAttracted;
    public bool isAttracted { get => _isAttracted; set => _isAttracted = value; }
    float _maxSpeed;
    public float maxSpeed { get => _maxSpeed; set => _maxSpeed = value; }
    float _initDamage;
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
        _weapon.bulletDamage *= 2;
        UIManager.Instance.MidScreenWarningText("Damage power up!", 1f);

        UIManager.Instance.DamagePowerUp.SetActive(true);
        Debug.Log("Damage: " + _weapon.bulletDamage);
    }

    public override void DeactivateEffect()
    {
        Debug.Log("Damage PowerUp Deactivated");
        _initDamage = _weapon.bulletDamage / 2;
        _weapon.bulletDamage = _initDamage;
        UIManager.Instance.DamagePowerUp.SetActive(false);

        Debug.Log("Damage: " + _weapon.bulletDamage);
    }


}
