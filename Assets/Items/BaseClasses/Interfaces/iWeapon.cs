using UnityEngine;

public interface iWeapon
{
    void OnEquip(Entity entity);
    void OnUnequip(Entity entity);

    void OnAttackTriggered(Entity entity);
    void OnAttackHeld(Entity entity);
    void OnAttackEnd(Entity entity);

    void OnHit(Collider2D other, Entity entity, GameObject AttackObject);
}