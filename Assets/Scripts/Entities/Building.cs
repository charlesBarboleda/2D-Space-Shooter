using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Faction))]
public class Building : MonoBehaviour
{

    [SerializeField] float health = 1000f;
    [SerializeField] List<GameObject> _turretChildren = new List<GameObject>();
    List<CircleCollider2D> _colliders = new List<CircleCollider2D>();
    SpriteRenderer _spriteRenderer;
    Faction _faction;

    bool _isDead;
    public bool isDead { get => _isDead; set => _isDead = value; }
    [SerializeField] List<string> _deathEffect = new List<string>();
    public List<string> deathEffect { get => _deathEffect; set => _deathEffect = value; }
    [SerializeField] string _deathExplosion;
    public string deathExplosion { get => _deathExplosion; set => _deathExplosion = value; }

    void Awake()
    {
        _faction = GetComponent<Faction>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _colliders.AddRange(GetComponents<CircleCollider2D>());
    }
    void OnEnable()
    {
        StartCoroutine(SpawnAnimation());
    }

    IEnumerator SpawnAnimation()
    {
        GameObject animation = ObjectPooler.Instance.SpawnFromPool("BuildingSpawn", transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        animation.SetActive(false);
    }



    public void TeleportAway()
    {
        GameManager.Instance.RemoveEnemy(gameObject, _faction);
        StartCoroutine(TeleportEffect());

    }

    IEnumerator TeleportEffect()
    {

        yield return StartCoroutine(SpawnAnimation());
        gameObject.SetActive(false);
    }


    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float health)
    {
        this.health = health;

    }



}
