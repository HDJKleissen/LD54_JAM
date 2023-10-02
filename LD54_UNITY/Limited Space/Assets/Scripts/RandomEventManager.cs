using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    [SerializeField] float minTimeBetweenEvents;
    [SerializeField] float maxTimeBetweenEvents;
    [SerializeField] PlayerMovement player;
    [SerializeField] GameObject piratePrefab, tumbleWeedPrefab, asteroidPrefab;
    float nextEventTime;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetNextTimer()
    {
        nextEventTime = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
    }
}
