using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Astar;
using System;

public class GameManager : MonoBehaviour {

    public static GameManager GM;
    public ProgressionManager progressionManager;
    public PlayerReference player;
    public Grid grid;

    public ItemPanel itemPanel;

    public GameObject deathScreen;

    [HideInInspector]
    public TileMapManager mapManager;
    [HideInInspector]
    public GameObject InGameObjects;
    public DefaultRNG Rng;
    public EnemyMetaDataSO enemyMetadata;
    public ItemsMetaData itemsMetaData;
    [HideInInspector]
    public OptionsManager optionsManager;
    [HideInInspector]
    public PauseManager pauseManager;
    [HideInInspector]
    public AudioManager audioManager;

    public LevelGenerator levelGenerator;


    public void Awake()
    {
        if (GM == null)
            GM = this;
        else
        if (GM != this)
            Destroy(gameObject);

        progressionManager = GetComponent<ProgressionManager>();
        player = GetComponent<PlayerReference>();
        grid = GetComponent<Grid>();

        DontDestroyOnLoad(gameObject);

        InGameObjects = new GameObject("InGameObjects");
        mapManager = GetComponent<TileMapManager>();
        optionsManager = GetComponent<OptionsManager>();
        pauseManager = GetComponent<PauseManager>();
        levelGenerator = GetComponent<LevelGenerator>();
        audioManager = GetComponent<AudioManager>();
    }

    public void PlayerDeath()
    {
        deathScreen.SetActive(true);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

}
