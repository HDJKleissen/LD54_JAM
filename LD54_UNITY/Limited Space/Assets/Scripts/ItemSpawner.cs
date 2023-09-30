using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemSpawner : MonoBehaviour
{
    private Planet planet;
    [SerializeField] CarriageItem itemToSpawn;
    [SerializeField] float randomOffSetRange = 3.0f;
    private void Awake()
    {
        planet = GetComponent<Planet>();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnItems(5));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SpawnItems(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            Sequence s = DOTween.Sequence();
            Vector3 randomOffset = new Vector3(Random.Range(-randomOffSetRange, randomOffSetRange), Random.Range(-randomOffSetRange, randomOffSetRange), Random.Range(-randomOffSetRange, randomOffSetRange));
            CarriageItem item = Instantiate(itemToSpawn, planet.ItemContainer.position, Quaternion.Euler(0,0, 0));
            s.Append(item.transform.DOMove(planet.ItemContainer.position + randomOffset, Random.Range(0.1f, 0.3f)).SetEase(Ease.InQuad));
            s.Join(item.transform.DORotate(new Vector3(0, 0, Random.Range(0, 360)), Random.Range(0.5f, 2f)).SetEase(Ease.OutSine));
            item.transform.SetParent(planet.ItemContainer, true);
        }
        yield return null;
    }
}
