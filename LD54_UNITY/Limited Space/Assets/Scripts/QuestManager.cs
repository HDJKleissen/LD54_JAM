using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    List<Planet> planets = new List<Planet>();
    private void Awake()
    {
        planets = FindObjectsOfType<Planet>().ToList();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
