using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class ItemsMetaData : ScriptableObject {
    [HideInInspector]
    public List<string> wepKeys;
    public List<WeaponSO> weapons;
    [HideInInspector]
    public List<string> consKeys;
    public List<ConsumableSO> consumeables;

    public void Awake ()
    {
        if (wepKeys == null)
        {
            wepKeys = new List<string>();
            weapons = new List<WeaponSO>();
            consKeys = new List<string>();
            consumeables = new List<ConsumableSO>();
        }
        
    }

#if UNITY_EDITOR
    public void AddItem(Item item, string Path)
    {
        if (item is Weapon)
        {
            string name = item.GetType().Name;
            if (wepKeys.Contains(name) == false)
            {
                wepKeys.Add(name);

                WeaponSO wep = (WeaponSO)ScriptableObject.CreateInstance(Type.GetType(name + "SO"));
                wep.name = name;
                weapons.Add(wep);

                AssetDatabase.CreateAsset(wep, Path + "/" + wep.name + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        else if (item is Consumable)
        {
            //if (consKeys.Contains(item.name) == false)
            //{
            //    consKeys.Add(item.name);
            //    consumeables.Add((Consumable)item);
            //}
        }
    }

    public void DeleteItem(string itemName)
    {
        if (wepKeys.Contains(itemName))
        {
            int i = wepKeys.IndexOf(itemName);
            wepKeys.RemoveAt(i);
            weapons.RemoveAt(i);
        }
        else if (consKeys.Contains(itemName))
        {
            int i = consKeys.IndexOf(itemName);
            consKeys.RemoveAt(i);
            consumeables.RemoveAt(i);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.SetDirty(this);
    }
#endif
}
