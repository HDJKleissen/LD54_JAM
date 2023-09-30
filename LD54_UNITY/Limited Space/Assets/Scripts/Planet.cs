using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    Transform player;
    public Transform ItemContainer;
    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
