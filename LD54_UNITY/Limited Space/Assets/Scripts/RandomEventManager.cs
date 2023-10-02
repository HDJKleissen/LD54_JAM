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
    [SerializeField] float playerDistance;
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
        playerDistance = Vector2.Distance(Vector2.zero, player.transform.position);
        if (playerDistance > 50)
        {
            possibleSpawnTypes = 1;
        }
        else if (playerDistance > 150)
        {
            possibleSpawnTypes = 2;
        }
        else if (playerDistance > 300)
        {
            possibleSpawnTypes = 3;
        }

        if (possibleSpawnTypes > 0)
        {
            int spawnType = Random.Range(1, possibleSpawnTypes + 1);
            switch (spawnType)
            {
                case 1:
                    Debug.Log("Spawning tumbleweeds");
                    break;
                case 2:
                    Debug.Log("Spawning asteroids");
                    break;
                case 3:
                    Debug.Log("Spawning pirate");
                    break;
            }
        }
    }

    void SetNextTimer()
    {
        timer = 0;
        nextEventTime = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
    }
}
