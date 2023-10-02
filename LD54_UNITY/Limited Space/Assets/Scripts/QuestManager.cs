using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    List<Planet> planets = new List<Planet>();

    [SerializeField] GameObject requirementPrefab;
    [SerializeField] Sprite cactusSprite;
    [SerializeField] Sprite boxSprite;
    [SerializeField] Sprite woodSprite;
    [SerializeField] Sprite briefcaseSprite;
    [SerializeField] Sprite tumbleweedSprite;

    [SerializeField] float timeBetweenQuests = 20;
    float timer;
    private void Awake()
    {
        planets = FindObjectsOfType<Planet>().ToList();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void GiveRandomPlanetARequirement()
    {
        Planet planet = planets[UnityEngine.Random.Range(0, planets.Count)];
        ItemType spawningItem = planet.GetComponent<ItemSpawner>().itemToSpawnType;

        if(planet.requirements.Count == 0)
        {
            List<int> possibleTypes = new List<int>();
            int itemTypeAmount = Enum.GetNames(typeof(ItemType)).Length;


            for (int i = 0; i < itemTypeAmount; i++)
            {
                if((ItemType)i != spawningItem)
                {
                    possibleTypes.Add(i);
                } 
            }

            Sprite newRequiredItemSprite = null;
            ItemType chosenType = (ItemType)possibleTypes[UnityEngine.Random.Range(0, possibleTypes.Count)];

            switch (chosenType)
            {
                case ItemType.Cactus:
                    newRequiredItemSprite = cactusSprite;
                    break;
                case ItemType.Crate:
                    newRequiredItemSprite = boxSprite;
                    break;
                case ItemType.Wood:
                    newRequiredItemSprite = woodSprite;
                    break;
                case ItemType.Briefcase:
                    newRequiredItemSprite = briefcaseSprite;
                    break;
                case ItemType.Tumbleweed:
                    newRequiredItemSprite = tumbleweedSprite;
                    break;
            }

            Debug.Log("Adding " + chosenType.ToString() + " to " + planet.name);
            planet.AddRandomRequirement(requirementPrefab, chosenType, newRequiredItemSprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenQuests)
        {
            GiveRandomPlanetARequirement();
            timer = 0;
        }
        CheckTotalPlanetRequirementsMet();
    }

    void CheckTotalPlanetRequirementsMet()
    {
        foreach(Planet planet in planets)
        {
            if (!planet.IsComplete)
            {
                return;
            }
        }

        Debug.LogWarning("COMPLETED ALL PLANET REQUIREMENTS!");
    }
}
