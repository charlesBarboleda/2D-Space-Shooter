using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour, IPickable
{
    [SerializeField] AudioClip _onPickUpAudio;
    AudioSource _audioSource;
    bool _isAttracted;
    public bool isAttracted { get => _isAttracted; set => _isAttracted = value; }
    float _maxSpeed;
    public float maxSpeed { get => _maxSpeed; set => _maxSpeed = value; }
    float currencyWorth;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
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
        PlayerManager.Instance.SetCurrency(PlayerManager.Instance.Currency() + currencyWorth);
        _audioSource.PlayOneShot(_onPickUpAudio);
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
