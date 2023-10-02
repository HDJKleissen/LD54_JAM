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

    [SerializeField] float distanceToOffscreen;
    [SerializeField] float tumbSpawnRadius;
    [SerializeField] float astSpawnRadius;
    [SerializeField] float pirateSpawnRadius;
    [SerializeField] float tumbMinVel;
    [SerializeField] float tumbMaxVel;
    [SerializeField] float astMinVel;
    [SerializeField] float astMaxVel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > nextEventTime)
        {
            SetNextTimer();
            if (!player.isNearPlanet && player.health > 0)
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
        if (playerDistance > 100)
        {
            possibleSpawnTypes = 2;
        }
        if (playerDistance > 150)
        {
            possibleSpawnTypes = 3;
        }

        if (possibleSpawnTypes > 0)
        {
            int spawnType = Mathf.Clamp(Random.Range(1, possibleSpawnTypes + 1),1,4);

            float rng = Random.Range(0, 1f);
            Vector3 dir = rng < 0.33f ? player.transform.right : rng < 0.66f ? -player.transform.right : player.transform.up;

            switch (spawnType)
            {
                case 1:
                    for (int i = 0; i < playerDistance / 50; i++)
                    {
                        Transform tumb = Instantiate(tumbleWeedPrefab).transform;
                        tumb.position = player.transform.position + player.transform.up * 10 +
                            dir * distanceToOffscreen
                            + new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), 0).normalized * tumbSpawnRadius;
                        tumb.GetComponent<Rigidbody2D>().velocity = (player.transform.position - tumb.position).normalized * Random.Range(tumbMinVel, tumbMaxVel);
                    }
                    break;
                case 2:
                    for (int i = 0; i < playerDistance / 100; i++)
                    {
                        Transform ast = Instantiate(asteroidPrefab).transform;
                        ast.position = player.transform.position + player.transform.up * 10 +
                            dir * distanceToOffscreen
                            + new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), 0).normalized * astSpawnRadius;
                        ast.GetComponent<Rigidbody2D>().velocity = (player.transform.position - ast.position).normalized * Random.Range(astMinVel, astMaxVel);
                    }
                    break;
                case 3:
                    for (int i = 0; i < playerDistance / 150; i++)
                    {
                        Transform pirate = Instantiate(piratePrefab).transform;
                        pirate.position = player.transform.position + player.transform.up * 10 +
                            dir * distanceToOffscreen
                            + new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), 0).normalized * pirateSpawnRadius;
                    }
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