using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour, IDamageable
{
    private CarriageManager carriageManager;
    public void Damage(float amount)
    {
        // Choose random amount of cargo items, then distribute damage. For now we log to console
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
