﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Astar;

using System.Reflection;


public class Entity : Destructable
{
    public GameObject lookDirObj;

    public float softCollisionSize;
    public float softCollisionForce;

    public bool flipSpriteBasedOnVel;

    public ScriptableAnimationController sac;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Collider2D col;
    [HideInInspector]
    public Vector2 lookDir;
    [HideInInspector]
    public Vector2 moveVector;
    [HideInInspector]
    public bool moving;
    [HideInInspector]
    public StateController controller;
    [HideInInspector]
    public bool attack;


    float stunLockTimer;
    public bool stunLocked;

    public Vector2 knockbackVector;

    [HideInInspector]
    public float dist;

    static Assembly asm;
    //Lists of all items that can be created.
    public ItemsMetaData items;

    //when the entity is created this creates whatever item the name is and equips it.
    public string weaponName;
    // Equiped Weapon;
    public Weapon wep;
    public List<Item> inventory;

    public iInteractable targetInteractable;

    //Position that a weapon attack will spawn at.
    [HideInInspector]
    public Vector2 atkPos;
    // distance away from the entity a weapon should spawn;
    public float atkSpawnDistance;




    void Awake()
    {
        if (asm == null)
        {
            asm = Assembly.Load("Assembly-CSharp");
        }
        inventory = new List<Item>();
    }


    // Use this for initialization
    public override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        controller = GetComponent<StateController>();
        base.Start();

        Equip(weaponName);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (PauseManager.gamePaused || !rend.isVisible)
            return;

        if (stunLockTimer > 0)
        {
            stunLocked = true;
            stunLockTimer -= Time.deltaTime;
            LerpStop();
        }
        else
        {
            stunLocked = false;
        }

        if (stunLocked)
            return;

        if (lookDirObj != null)
            UpdateLookDirObj();

        // atkPos is used to position the weapon that gets spwned when an entity triggers a weapon to be used;
        // curently it uses the lookDir and just moves it out by some Distance;
        atkPos = lookDir * atkSpawnDistance;
    }

    void FixedUpdate()
    {
        if (PauseManager.gamePaused)
            return;

        CheckForSoftCollisions(softCollisionSize, softCollisionForce);

        rb.velocity = moveVector;
        if(rb.velocity.magnitude > 0)
        {
            if (rb.velocity.magnitude > stats.moveSpeed)
                rb.velocity = rb.velocity.normalized * stats.moveSpeed;
        }

        UpdateKnockback();


        Animate();

        
        if (gameObject.name != "Player")
        {
            dist = Vector2.Distance(transform.position, Camera.main.transform.position);
            if (dist >= (Camera.main.orthographicSize * 2)+6)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void LerpStop()
    {
        moveVector = Vector2.Lerp(moveVector, Vector2.zero, stats.knockbackDeceleration*Time.deltaTime);
    }

    public void StunLock(float time)
    {
        stunLockTimer += time;
    }

    public void Animate()
    {
        if(sac != null)
            sac.Animate(this);

        if(flipSpriteBasedOnVel)
        {
            if(moveVector.magnitude > 0)
            {
                if(moveVector.x > 0)
                {
                    rend.flipX = false;
                }
                else
                if(moveVector.x < 0)
                {
                    rend.flipX = true;
                }
            }
        }

        if (attack)
            attack = false;


    }

    public void CheckForSoftCollisions(float size, float pushForce)
    {
        Vector2 additiveMoveVector = Vector2.zero;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, size, stats.softCollisions);
        if(cols.Length >0)
        {
            foreach (Collider2D col in cols)
            {
                additiveMoveVector += (Vector2)(transform.position-col.transform.position);
            }
            additiveMoveVector /= cols.Length;

            if (additiveMoveVector.magnitude > 1)
                additiveMoveVector.Normalize();

            rb.velocity = rb.velocity + (additiveMoveVector * pushForce);
        }
    }

    public void UpdateKnockback()
    {
        if(knockbackVector.magnitude > 0)
        {
            knockbackVector = Vector2.Lerp(knockbackVector, Vector2.zero, stats.knockbackDeceleration*Time.deltaTime);
            rb.velocity += knockbackVector;
        }
    }

    public void AddKnockback(Vector2 _knockbackVector)
    {
        knockbackVector += _knockbackVector;
    }

    public void UpdateLookDirObj()
    {
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        lookDirObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }


    public void Equip(string Name)
    {
        if (items.wepKeys.Contains(Name))
        {
            // this is looking in all the objects to find a item then spawns the runtime counterpart and initalize it.
            // sets the object we created to the current wep variable
            // and runs its OnEquipfunction;
            WeaponSO wepSO = items.weapons[items.wepKeys.IndexOf(Name)];
            wep = (Weapon)asm.CreateInstance(Name);
            wep._itemData = wepSO;
            wep.Start(this);
            wep.OnEquip(this);
        }
        else
        {
            Debug.Log("Item is not equipable.");
        }
    }

    public void Equip(Weapon Weapon)
    {
        wep = Weapon;
        wep.OnEquip(this);
    }

    public void UnEquip() { }

    public void Interact()
    {
        targetInteractable.Interact(this);
    }



}
