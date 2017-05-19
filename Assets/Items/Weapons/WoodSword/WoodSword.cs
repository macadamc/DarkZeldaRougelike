using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WoodSword : Weapon {
    
    // we use this variable to cast the base ItemSO into whatever weaponSO it actualy is. in this case its the WoodSwordSO.
    WoodSwordSO itemData;

    public override void Start (Entity entity)
    {
        itemData = (WoodSwordSO)_itemData; // BoilerPlate : every item has to do this in start for there own ItemType.
    }

    public override void OnEquip(Entity entity)
    {
        //Debug.Log(entity.name + " equiped a Wooden Sword!");
    }

    public override void OnUnequip(Entity entity)
    {
        throw new NotImplementedException();
    }

    public override void OnAttackTriggered(Entity entity)
    {
        //create the "weapon swing" gameobject.
        GameObject g = (GameObject)GameObject.Instantiate(itemData.prefabs[0], entity.gameObject.transform);
        //set the attacks position and rotation to match the entitys atkPos and.. TODO : look direction?
        g.transform.localPosition = entity.atkPos;
        g.transform.rotation = GetQuaternionFromEntityLookDirection(entity);
        entity.StunLock(itemData.AttackDelay);
        entity.attack = true;
    }

    public override void OnAttackHeld(Entity entity)
    {
    }

    public override void OnAttackEnd(Entity entity)
    {
    }

    public override void OnHit(Collider2D other, Entity entity, GameObject AttackObject)
    {
        //test to see if the other collider is a entity.
        Entity e = other.GetComponent<Entity>();
        Destructable d = other.GetComponent<Destructable>();
        if (e != null && e != entity)
        {
            GameManager.GM.pauseManager.StartCoroutine(GameManager.GM.pauseManager.HitPause(0.05f));
            e.AddKnockback((e.transform.position - entity.transform.position).normalized * itemData.knowckBack);
            //e.AddKnockback(entity.lookDir * itemData.knowckBack);
            e.StunLock(itemData.knowckBack / 20);   //should have stunlock value in weapon maybe
            e.ModifyHealth(-itemData.baseDamage);
            return;
        }
        else if (d != null)
        {
            d.ModifyHealth(-itemData.baseDamage);
        }
    }
}