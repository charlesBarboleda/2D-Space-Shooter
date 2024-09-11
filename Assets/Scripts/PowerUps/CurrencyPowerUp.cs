using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPowerUp : PowerUp, IPickable
{
    [SerializeField] float _currencyAmount = 2000f;
    bool _isAttracted;
    public bool isAttracted { get => _isAttracted; set => _isAttracted = value; }
    float _maxSpeed;
    public float maxSpeed { get => _maxSpeed; set => _maxSpeed = value; }
    float _initSpeed;
    PlayerManager _player;
    void Start()
    {
        _player = PlayerManager.Instance;
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
        Debug.Log("Currency PowerUp Activated");

        _player.SetCurrency(_player.Currency() + _currencyAmount);
        Debug.Log("Speed: " + _player.MoveSpeed());
    }

    public override void DeactivateEffect()
    {

    }


}
