using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class PlanetInstantiater : MonoBehaviour
{
    [Header("REFRESH CLICK HERE IN EDIT MODO")]
    [SerializeField] bool RUNCHANGES = false;
    public List<GameObject> planetPrefabs = new List<GameObject>();
    public List<Color> recolors = new List<Color>();

    [SerializeField] float minScale = 0.3f;
    [SerializeField] float maxScale = 2;

    [SerializeField] float chanceForStore = 10.0f;

    public List<Planet> planets = new List<Planet>();
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
        planets.Clear();

        List<Vector2> planetPosses = GeneratePoissonPoints();

        // Loop through all children and destroy them.
        for (int i = this.transform.childCount; i > 0; --i)
            DestroyImmediate(this.transform.GetChild(0).gameObject);

        for (int i = 0; i < planetPosses.Count; i++)
        {
            Instantiate(planetPrefabs.GetRandomValue(), new Vector3(planetPosses[i].x, planetPosses[i].y, 0) + this.transform.position, Quaternion.identity, this.transform);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform tform = transform.GetChild(i);
            planets.Add(tform.GetComponentInChildren<Planet>());
        }

        foreach(Planet p in planets)
        {
            // rescale
            float scale = UnityEngine.Random.Range(minScale, maxScale);
            p.transform.localScale = new Vector3(scale, scale, scale);

            //recolor
            Color color = recolors.GetRandomValue();
            p.GetComponent<SpriteRenderer>().color = color;

            //enable store possibly
            if(UnityEngine.Random.Range(0.0f, 1.0f) < chanceForStore)
            {
                Transform shopTransform = p.transform.parent.GetComponentInChildren<Shop>(true).transform;
                shopTransform.gameObject.SetActive(true);
                p.transform.parent.name = "PLANET_WITH_STORE";

                Vector2 shopOffsets = new Vector3(-4, 4) * p.transform.parent.localScale.x;
                shopTransform.localPosition = new Vector3(shopOffsets.GetRandomValueInRange(), shopOffsets.GetRandomValueInRange(), 0);
            }
        }
    }

    [Header("PLANET SPAWNWER")]
    public int gridWidth = 100;
    public int gridHeight = 100;
    public float cellSize = 10f;
    public float minDistance = 20.0f;

    private int[,] grid;

    public List<Vector2> GeneratePoissonPoints()
    {
        grid = new int[gridWidth, gridHeight];
        List<Vector2> points = new List<Vector2>();
        List<Vector2> activeList = new List<Vector2>();

        Vector2 initialPoint = new Vector2(gridWidth / 2.0f, gridHeight / 2.0f) * cellSize;
        activeList.Add(initialPoint);
        points.Add(initialPoint);

        while (activeList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, activeList.Count);
            Vector2 currentPoint = activeList[randomIndex];
            bool isValid = false;

            for (int i = 0; i < 30; i++)
            {
                float angle = 2 * Mathf.PI * UnityEngine.Random.value;
                float distance = minDistance + (minDistance * UnityEngine.Random.value);

                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                Vector2 newPoint = currentPoint + direction * distance;

                if (newPoint.x >= 0 && newPoint.x < gridWidth * cellSize && newPoint.y >= 0 && newPoint.y < gridHeight * cellSize)
                {
                    int gridX = Mathf.FloorToInt(newPoint.x / cellSize);
                    int gridY = Mathf.FloorToInt(newPoint.y / cellSize);

                    bool isValidCandidate = true;

                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            int neighborX = gridX + dx;
                            int neighborY = gridY + dy;

                            if (neighborX >= 0 && neighborX < gridWidth && neighborY >= 0 && neighborY < gridHeight)
                            {
                                if (grid[neighborX, neighborY] != 0 &&
                                    Vector2.Distance(newPoint, points[grid[neighborX, neighborY] - 1]) < minDistance)
                                {
                                    isValidCandidate = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (isValidCandidate)
                    {
                        isValid = true;
                        activeList.Add(newPoint);
                        points.Add(newPoint);

                        int newGridX = Mathf.FloorToInt(newPoint.x / cellSize);
                        int newGridY = Mathf.FloorToInt(newPoint.y / cellSize);
                        grid[newGridX, newGridY] = points.Count;
                        break;
                    }
                }
            }

            if (!isValid)
            {
                activeList.RemoveAt(randomIndex);
            }
        }

        // Now, 'points' contains the generated Vector2 points.
        foreach (var point in points)
        {
            Debug.Log("Point: " + point);
        }

        return points;
    }
}
