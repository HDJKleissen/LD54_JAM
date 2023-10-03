using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : Hazard, IDamageable
{
    protected override DamageSource damageSource => DamageSource.Container;

    public float maxHealth = 15;
    public float health = 15;

    private CarriageManager carriageManager;
    [SerializeField] SpriteRenderer sprenderer;

    public void Damage(float amount, DamageSource source)
    {
        // Amount is usually 0-10ish with 10 being a hard hit

        switch (source)
        {
            case DamageSource.None:
                break;
            case DamageSource.Asteroid:
                if(amount > 10f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Asteroid Impact");
                    // SFX: Very Hard asteroid hit
                }
                else if (amount > 7.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Asteroid Impact");
                    // SFX: Hard asteroid hit
                }
                else if (amount > 5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Asteroid Impact");
                    // SFX: Medium asteroid hit
                }
                else if (amount > 2.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Asteroid Impact");
                    // SFX: Light asteroid hit
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Asteroid Impact");
                    // SFX: Very Light asteroid hit
                }
                break;
            case DamageSource.Pirate:
                if (amount > 10f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Pirate Impact");
                    // SFX: Very Hard Pirate hit
                }
                else if (amount > 7.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Pirate Impact");
                    // SFX: Hard Pirate hit
                }
                else if (amount > 5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Pirate Impact");
                    // SFX: Medium Pirate hit
                }
                else if (amount > 2.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Pirate Impact");
                    // SFX: Light Pirate hit
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Pirate Impact");
                    // SFX: Very Light Pirate hit
                }
                break;
            case DamageSource.Tumbleweed:
                if (amount > 10f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Tumbleweed Impact");
                    // SFX: Very Hard Tumbleweed hit
                }
                else if (amount > 7.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Tumbleweed Impact");
                    // SFX: Hard Tumbleweed hit
                }
                else if (amount > 5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Tumbleweed Impact");
                    // SFX: Medium Tumbleweed hit
                }
                else if (amount > 2.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Tumbleweed Impact");
                    // SFX: Light Tumbleweed hit
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Tumbleweed Impact");
                    // SFX: Very Light Tumbleweed hit
                }
                // SFX: Tumbleweed hit, can use amount for intensity or sth
                break;
            default:
                if (amount > 10f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Carriage Impact");
                    // SFX: Very Hard Default hit
                }
                else if (amount > 7.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Carriage Impact");
                    // SFX: Hard Default hit
                }
                else if (amount > 5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Carriage Impact");
                    // SFX: Medium Default hit
                }
                else if (amount > 2.5f)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Carriage Impact");
                    // SFX: Light Default hit
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Carriage Impact");
                    // SFX: Very Light Default hit
                }
                // SFX: Default hit, can use amount for intensity or sth
                break;
        }

        health -= amount;
        if(health < 0)
        {
            sprenderer.color = new Color(.5f, .5f, .5f);
        }
    }

    internal void RepairFull()
    {
        health = maxHealth;
        sprenderer.color = new Color(1, 1, 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        carriageManager = GetComponentInChildren<CarriageManager>(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(InventoryManager inventoryManager)
    {
        if (health > 0)
        {
            inventoryManager.SetCarriageState(carriageManager);
        }
    }
}
