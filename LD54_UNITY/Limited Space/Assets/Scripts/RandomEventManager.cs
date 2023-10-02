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
        timer += Time.deltaTime;
        if(timer > nextEventTime)
        {
            SetNextTimer();
            if(!player.isNearPlanet)
            {
                SpawnEvent();
            }
        }
    }

    void SpawnEvent()
    {
        int possibleSpawnTypes = 0;
        float distanceFromCenter = Vector2.Distance(Vector2.zero, player.transform.position);
        if (distanceFromCenter > 50)
        {
            possibleSpawnTypes = 1;
        }
        else if (distanceFromCenter > 300)
        {
            possibleSpawnTypes = 2;
        }
        else if (distanceFromCenter > 600)
        {
            possibleSpawnTypes = 3;
        }

        if (possibleSpawnTypes > 0)
        {
            int spawnType = Random.Range(1, possibleSpawnTypes + 1);
            switch (spawnType)
            {

            }
        }
    }

    void SetNextTimer()
    {
        nextEventTime = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
    }
}
