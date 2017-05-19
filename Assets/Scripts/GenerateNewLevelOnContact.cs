using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ShadyPixel.Astar;

public class GenerateNewLevelOnContact : MonoBehaviour {

    GameManager gm;

	void Awake () {
        gm = GameObject.Find("Manager").GetComponent<GameManager>();
        GetComponent<Collider2D>().isTrigger = true;
	}
	
	// Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            gm.mapManager.map.ClearData();
            
            for(int i = 0; i < gm.InGameObjects.transform.childCount;i++)
            {
                Destroy(gm.InGameObjects.transform.GetChild(i).gameObject);
            }
            gm.levelGenerator.forestGenerator.Generate();
            gm.mapManager.cManager.UpdateChunks(true);// true forces the whole map to be regenerated.
            gm.GetComponent<Grid>().Start();
        }
    }
}
