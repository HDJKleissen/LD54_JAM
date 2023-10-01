using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour, IDamageable
{
    private CarriageManager carriageManager;
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
                    // SFX: Very Hard asteroid hit
                }
                else if (amount > 7.5f)
                {
                    // SFX: Hard asteroid hit
                }
                else if (amount > 5f)
                {
                    // SFX: Medium asteroid hit
                }
                else if (amount > 2.5f)
                {
                    // SFX: Light asteroid hit
                }
                else
                {
                    // SFX: Very Light asteroid hit
                }
                break;
            case DamageSource.Pirate:
                if (amount > 10f)
                {
                    // SFX: Very Hard Pirate hit
                }
                else if (amount > 7.5f)
                {
                    // SFX: Hard Pirate hit
                }
                else if (amount > 5f)
                {
                    // SFX: Medium Pirate hit
                }
                else if (amount > 2.5f)
                {
                    // SFX: Light Pirate hit
                }
                else
                {
                    // SFX: Very Light Pirate hit
                }
                break;
            case DamageSource.Tumbleweed:
                if (amount > 10f)
                {
                    // SFX: Very Hard Tumbleweed hit
                }
                else if (amount > 7.5f)
                {
                    // SFX: Hard Tumbleweed hit
                }
                else if (amount > 5f)
                {
                    // SFX: Medium Tumbleweed hit
                }
                else if (amount > 2.5f)
                {
                    // SFX: Light Tumbleweed hit
                }
                else
                {
                    // SFX: Very Light Tumbleweed hit
                }
                // SFX: Tumbleweed hit, can use amount for intensity or sth
                break;
            default:
                if (amount > 10f)
                {
                    // SFX: Very Hard Default hit
                }
                else if (amount > 7.5f)
                {
                    // SFX: Hard Default hit
                }
                else if (amount > 5f)
                {
                    // SFX: Medium Default hit
                }
                else if (amount > 2.5f)
                {
                    // SFX: Light Default hit
                }
                else
                {
                    // SFX: Very Light Default hit
                }
                // SFX: Default hit, can use amount for intensity or sth
                break;
        }

        // Choose random amount of cargo items to delete, depending on damage. For now we log to console
        Debug.Log(name + ": ow (" + amount + ")");
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
        inventoryManager.SetCarriageState(carriageManager);
    }
}
