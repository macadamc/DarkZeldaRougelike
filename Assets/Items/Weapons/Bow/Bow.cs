using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    public BowSO itemData;
    GameObject HeldSprite;

    public override void Start(Entity entity)
    {
        itemData = (BowSO)_itemData;
        
    }

    public override void OnAttackEnd(Entity entity)
    {
        GameObject arrow = (GameObject)GameObject.Instantiate(itemData.prefabs[1], entity.transform);
        arrow.transform.localPosition = entity.atkPos;
        arrow.GetComponent<WeaponAttack>().owner = entity;
        arrow.GetComponent<DestroyAfterTime>().time = itemData.MaxArrowFlightTime;
        arrow.GetComponent<Rigidbody2D>().velocity = (entity.lookDir * itemData.arrowSpeed) + entity.moveVector;

        if (HeldSprite != null)
        {
            GameObject.Destroy(HeldSprite);
        }
        
    }

    public override void OnAttackHeld(Entity entity)
    {
        if (HeldSprite != null)
        {
            HeldSprite.transform.localPosition = entity.atkPos;
            HeldSprite.transform.rotation = GetQuaternionFromEntityLookDirection(entity);
        }
        
    }

    public override void OnAttackTriggered(Entity entity)
    {
        if (HeldSprite != null)
        {
            OnAttackEnd(entity);
        }

        HeldSprite = (GameObject)GameObject.Instantiate(itemData.prefabs[0], entity.gameObject.transform);
        HeldSprite.transform.localPosition = entity.atkPos;
        HeldSprite.transform.rotation = GetQuaternionFromEntityLookDirection(entity);
    }

    public override void OnEquip(Entity entity)
    {
        //throw new NotImplementedException();
    }

    public override void OnHit(Collider2D other, Entity entity, GameObject AttackObject)
    {
        Entity e = other.GetComponent<Entity>();
        if (e != null && e != entity)
        {
            e.ModifyHealth(-itemData.baseDamage);
        }

        if (other.gameObject.name.Contains("bush"))
        {
            GameObject.Destroy(other.gameObject);
        }
        GameObject.Destroy(AttackObject);
    }

    public override void OnUnequip(Entity entity)
    {
        //throw new NotImplementedException();
    }
}
