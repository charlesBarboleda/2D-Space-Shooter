using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : Enemy
{
    [SerializeField] float _buffRadius = 20f;
    [SerializeField] List<Enemy> _buffedAllies = new List<Enemy>();

    protected override void Update()
    {
        base.Update();
        BuffAllies();
        UnBuffAllies();
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
        for (int i = 0; i < _buffedAllies.Count; i++)
        {
            Enemy _enemy = _buffedAllies[i];
            if (Vector2.Distance(transform.position, _enemy.transform.position) > _buffRadius)
            {
                _buffedAllies[i].UnBuffedState();
                _buffedAllies.RemoveAt(i);
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
