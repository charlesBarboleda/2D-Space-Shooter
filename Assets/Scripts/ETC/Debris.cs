using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour, IPickable
{

    AudioSource _audioSource;
    bool _isAttracted;
    public bool isAttracted { get => _isAttracted; set => _isAttracted = value; }
    float _maxSpeed;
    public float maxSpeed { get => _maxSpeed; set => _maxSpeed = value; }
    float currencyWorth;
    Collider2D _colliders;

    void Awake()
    {
        _colliders = GetComponent<Collider2D>();
        _colliders.enabled = true;
    }

    void OnEnable()
    {
        _colliders.enabled = true;
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
        _colliders.enabled = false;
        PlayerManager.Instance.SetCurrency(PlayerManager.Instance.Currency() + currencyWorth);
        gameObject.SetActive(false);
    }

    public void SetCurrency(float currency)
    {
        this.currencyWorth = currency;
    }

    public void MoveTowardsPlayer()
    {
        Vector2 playerPosition = PlayerManager.GetInstance().transform.position;
        transform.position = Vector2.MoveTowards(transform.position, playerPosition, _maxSpeed * Time.deltaTime);
    }

}
