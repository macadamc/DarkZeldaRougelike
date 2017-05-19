using UnityEngine;

[System.Serializable]
public abstract class Consumable : Item, iUsable
{
    public abstract void Use(Entity entity);
}