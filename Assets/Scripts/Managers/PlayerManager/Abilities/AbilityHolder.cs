using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    [SerializeField] private List<Ability> abilities;

    [SerializeField] private KeyCode key;
    [SerializeField] private Transform target;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            abilities[0].UseAbility(gameObject, target);
        }
    }
}
