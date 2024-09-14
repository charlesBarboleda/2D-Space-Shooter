using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : Enemy
{
    [SerializeField] float _buffRadius = 20f;
    [SerializeField] List<Enemy> _buffedAllies = new List<Enemy>();

    protected override void Update()
    {
        Attack();
        UnBuffAllies();
        base.Update();
    }

    protected override void Attack()
    {
        BuffAllies();
    }

    void BuffAllies()
    {
        LayerMask layerMasks = LayerMask.GetMask("CrimsonFleet");
        Collider2D[] allies = Physics2D.OverlapCircleAll(transform.position, _buffRadius, layerMasks);
        foreach (Collider2D ally in allies)
        {
            Enemy _enemy = ally.GetComponent<Enemy>();
            if (!_buffedAllies.Contains(_enemy))
            {
                _enemy.BuffedState();
                _buffedAllies.Add(_enemy);
            }
        }

    }

    void UnBuffAllies()
    {
        List<Enemy> _buffedAlliesCopy = new List<Enemy>(_buffedAllies);
        foreach (Enemy ally in _buffedAlliesCopy)
        {
            if (Vector2.Distance(transform.position, ally.transform.position) > _buffRadius || Health.isDead)
            {
                ally.UnBuffedState();
                _buffedAllies.Remove(ally);
            }
        }
    }

    public override void BuffedState()
    {
        base.BuffedState();
        _buffRadius = _buffRadius * 1.5f;
    }

    public override void UnBuffedState()
    {
        base.UnBuffedState();
        _buffRadius = _buffRadius / 1.5f;
    }

}
