using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [field: SerializeField] public Transform WorldItemsContainer { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenCarriage(CarriageManager carriage)
    {
        carriage.gameObject.SetActive(true);
    }

}
