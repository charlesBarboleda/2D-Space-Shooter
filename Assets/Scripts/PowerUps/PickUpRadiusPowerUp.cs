using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRadiusPowerUp : PowerUp, IPickable
{
    bool _isAttracted;
    public bool isAttracted { get => _isAttracted; set => _isAttracted = value; }
    float _maxSpeed;
    public float maxSpeed { get => _maxSpeed; set => _maxSpeed = value; }
    float _initPickUpRadius;
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


        _player.PickUpRadius *= 10;

    }

    public override void DeactivateEffect()
    {

        _initPickUpRadius = _player.PickUpRadius / 10;
        _player.PickUpRadius = _initPickUpRadius;
    }


}
