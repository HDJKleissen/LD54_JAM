using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class PlanetInstantiater : MonoBehaviour
{
    public List<Planet> planets = new List<Planet>();

    [SerializeField] bool RUNCHANGES = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(RUNCHANGES == true)
        {
            ChangeWorlds();
            RUNCHANGES = false;
        }
    }

    void ChangeWorlds()
    {
        foreach(Transform t in transform)
        {
            planets.Add(transform.GetComponentInChildren<Planet>());
        }

        foreach(Planet p in planets)
        {
            float scale = Random.Range(0.3f, 2.0f);
            p.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
