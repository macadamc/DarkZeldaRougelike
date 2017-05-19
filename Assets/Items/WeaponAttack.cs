using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WeaponAttack : MonoBehaviour {
    [HideInInspector]
    public Entity owner;

    void Awake()
    {
        if (owner == null)
        {
            owner = transform.parent.GetComponent<Entity>();
            GetComponent<Collider2D>().isTrigger = true;
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        owner.wep.OnHit(other, owner, gameObject);
    }
}
