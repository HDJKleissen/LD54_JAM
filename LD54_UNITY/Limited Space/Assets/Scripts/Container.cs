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
                // SFX: Asteroid hit, can use amount for intensity or sth
                break;
            case DamageSource.Pirate:
                // SFX: Pirate hit, can use amount for intensity or sth
                break;
            case DamageSource.Tumbleweed:
                // SFX: Tumbleweed hit, can use amount for intensity or sth
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
