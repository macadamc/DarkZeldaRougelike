﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Destructable : ZOrderObject {

    public float health = 0f;
    public Stats stats;

    public bool skipDamageFlash;

    public AudioClip hurtSfx;
    AudioSource audioSource;

    public System.Random random = new System.Random();

    public void ModifyHealth(float changeInHealth)
    {
        health += changeInHealth;


        if (changeInHealth < 0)
        {
            PlayHurtSFX();
            StartCoroutine(DamageFlash(2));
        }
    }

    public void CheckForDeath()
    {
        if (health <= 0)
        {
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        Spawn(stats.spawnOnDeath);
        gameObject.SetActive(false);
    }

    public override void Start()
    {
        base.Start();
        health = stats.health;
        Spawn(stats.spawnOnCreate);
        audioSource = GetComponent<AudioSource>();
    }


    public void PlayHurtSFX()
    {
        audioSource.Stop();
        audioSource.clip = hurtSfx;
        audioSource.Play();
    }

    public void PlaySfx(AudioClip sfx)
    {
        audioSource.Stop();
        audioSource.clip = sfx;
        audioSource.Play();
    }

    public void Spawn(SpawnObject[] spawnObjects)
    {
        for (int i = 0; i < spawnObjects.Length; i++)
        {
            Vector2 randomVector = Random.insideUnitCircle * spawnObjects[i].positionRandomness;
            Vector3 spawnPos = (Vector3)(randomVector + spawnObjects[i].positionOffset) + transform.position;

            GameObject obj = Instantiate(spawnObjects[i].objectToSpawn, spawnPos, transform.rotation) as GameObject;
        }
    }

    public IEnumerator DamageFlash(int flashes)
    {
        if(!skipDamageFlash)
        {
            for (int i = 0; i < flashes; i++)
            {
                rend.enabled = true;
                yield return new WaitForSeconds(0.1f);
                rend.enabled = false;
                yield return new WaitForSeconds(0.1f);
            }
        }

        rend.enabled = true;
        CheckForDeath();
        yield return null;
    }
}
