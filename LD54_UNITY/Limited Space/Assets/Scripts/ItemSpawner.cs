using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemSpawner : MonoBehaviour
{
    private Planet planet;
    public CarriageItem itemToSpawn;
    [SerializeField] float randomOffSetRange = 3.0f;
    [SerializeField] int initialAmount = 5;
    [SerializeField] int maxAmount = 15;
    [SerializeField] int timedSpawnAmount = 5;
    [SerializeField] float timeBetweenSpawn= 5;

    float timer;

    private void Awake()
    {
        planet = GetComponent<Planet>();
    }

    void Start()
    {
        StartCoroutine(SpawnItems(initialAmount));
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > timeBetweenSpawn)
        {
            timer = 0;
            StartCoroutine(SpawnItems(timedSpawnAmount));
        }
    }

    public IEnumerator SpawnItems(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            if (planet.items.Count < maxAmount)
            {
                Sequence s = DOTween.Sequence();
                Vector3 randomOffset = new Vector3(Random.Range(-randomOffSetRange, randomOffSetRange), Random.Range(-randomOffSetRange, randomOffSetRange), Random.Range(-randomOffSetRange, randomOffSetRange));
                CarriageItem item = Instantiate(itemToSpawn, planet.ItemContainer.position, Quaternion.Euler(0, 0, 0));
                s.Append(item.transform.DOMove(planet.ItemContainer.position + randomOffset, Random.Range(0.1f, 0.3f)).SetEase(Ease.InQuad));
                s.Join(item.transform.DORotate(new Vector3(0, 0, Random.Range(0, 360)), Random.Range(0.5f, 2f)).SetEase(Ease.OutSine));
                item.transform.SetParent(planet.ItemContainer, true);
            }
            yield return null;
        }
        yield return null;
    }
}
