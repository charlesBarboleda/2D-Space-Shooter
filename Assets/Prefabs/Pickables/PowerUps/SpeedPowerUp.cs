using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUp, IPickable
{
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
        Debug.Log("Speed PowerUp Activated");

        _player.SetMoveSpeed(_initSpeed * 2);
        Debug.Log("Speed: " + _player.MoveSpeed());
    }

    public override void DeactivateEffect()
    {
        Debug.Log("Speed PowerUp Deactivated");
        _initSpeed = _player.MoveSpeed() / 2;
        _player.SetMoveSpeed(_initSpeed);
        Debug.Log("Speed: " + _player.MoveSpeed());
    }


}
