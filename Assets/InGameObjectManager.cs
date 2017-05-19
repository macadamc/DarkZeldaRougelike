using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameObjectManager : MonoBehaviour {

    public Transform Enemys;
    public Transform TerrainObjects;
    public Transform Chests;
    public Transform Spawners;

    Transform[] types;
    string[] names;

    public void Awake()
    {
        types = new Transform[4] { Enemys, TerrainObjects, Chests, Spawners };
        names = new string[4] { "Enemys", "TerrainObjects", "Chests", "Spawners" };
    }

    public void Start()
    {
        InitalizeGameObjectContainers();
    }

    public void InitalizeGameObjectContainers()
    {
        
        for (int i = 0; i < names.Length; i++)
        {
            Transform child = gameObject.transform.FindChild(names[i]);
            if (child == null)
            {
                GameObject go = new GameObject(names[i]);
                go.transform.parent = gameObject.transform;
                types[i] = go.transform;
            }
        }
    }

    public void DestroyAllGameObjects ()
    {
        foreach(Transform t in types)
        {
            for(int i = 0; i < t.childCount; i++)
            {
                Destroy(t.gameObject);
            }
        }
    }
}
