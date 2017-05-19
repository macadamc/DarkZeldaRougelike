﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;

[RequireComponent(typeof(Entity))]
public class PlayerController : MonoBehaviour
{

    Entity entity;
    public bool strafe;
    bool attackBool;
    public float deadZone = 0.2f;
    public bool dead;

    void Start()
    {
        entity = GetComponent<Entity>();
    }

	// Update is called once per frame
	void Update ()
    {
        if (PauseManager.gamePaused)
            return;

        CheckForInteractable();

        CheckForAttacks();

        SetMoveVector();

        SetLookDir();

        if(entity.health <= 0 && !dead)
        {
            dead = true;
            Invoke("PlayerDead", 2f);
        }
    }

    void PlayerDead()
    {
        GameManager.GM.PlayerDeath();
    }

    void SetMoveVector()
    {
        Vector2 input = new Vector2(CnInputManager.GetAxis("Horizontal"), CnInputManager.GetAxis("Vertical"));

        if (input.magnitude < deadZone) { input = Vector2.zero; }

        entity.moveVector = input * entity.stats.moveSpeed;
    }

    void SetLookDir()
    {
        if (strafe == true || CnInputManager.GetButton("Fire2") == true || entity.stunLocked)
        {
            return;
        }

        Vector2 input = new Vector2(CnInputManager.GetAxis("Horizontal"), CnInputManager.GetAxis("Vertical"));

        if (input.magnitude < deadZone) { input = Vector2.zero; }


        if (input.magnitude > 0)
        {
            Vector2 lookInput = input;
            entity.lookDir = lookInput.normalized;
        }
    }

    void CheckForInteractable()
    {
        if(entity.targetInteractable != null)
        {

            if (CnInputManager.GetButtonDown("Fire1"))
            {
                entity.Interact();
            }
        }
    }

    void CheckForAttacks()
    {
        bool onDown = CnInputManager.GetButtonDown("Fire2");
        if (onDown && entity.wep != null)
        {
            entity.wep.OnAttackTriggered(entity);
            attackBool = true;
        }
        if (onDown == false && CnInputManager.GetButton("Fire2") && entity.wep != null)
        {
            entity.wep.OnAttackHeld(entity);
        }
        if (CnInputManager.GetButton("Fire2") == false && attackBool && entity.wep != null)
        {
            entity.wep.OnAttackEnd(entity);
            attackBool = false;
        }
    }
}
