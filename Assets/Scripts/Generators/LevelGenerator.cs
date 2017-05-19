using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public CaveGen forestGenerator;

    void Start()
    {
        GameManager.GM.Rng = new DefaultRNG(System.DateTime.Now.GetHashCode());
        forestGenerator.Init(GameManager.GM.mapManager, GameManager.GM.Rng, GameManager.GM.enemyMetadata);
        forestGenerator.Generate();
    }
}
