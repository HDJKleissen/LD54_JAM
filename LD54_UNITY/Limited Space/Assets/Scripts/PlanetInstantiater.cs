using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class PlanetInstantiater : MonoBehaviour
{
    public List<Transform> planets = new List<Transform>();

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
        planets = GetComponentsInChildren<Transform>().ToList();

        foreach(Transform p in planets)
        {
            float scale = Random.Range(0.3f, 2.0f);
            p.localScale = new Vector3(scale, scale, scale);
        }
    }
}
