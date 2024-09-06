using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour
{
    [SerializeField] List<Transform> _targets;
    [SerializeField] float _speed = 5f;


    void Update()
    {
        int RandomTarget = Random.Range(0, _targets.Count);
        transform.position = Vector3.MoveTowards(transform.position, _targets[RandomTarget].position, _speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _targets[RandomTarget].position) < 1f)
        {
            gameObject.SetActive(false);
        }
    }

    public float Speed { get => _speed; set => _speed = value; }


}
