using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Planet : MonoBehaviour
{
    Transform player;
    public Transform ItemContainer;
    public List<CarriageItem> items = new List<CarriageItem>();
    public List<PlanetRequirement> requirements = new ();

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        requirements = FindObjectsOfType<PlanetRequirement>().ToList();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // nice performance lol, the adding and removing of objects is too shitty so have to do it like dies
        CheckRequirements();
    }

    void CheckRequirements()
    {
        List<int> usedIndices = new List<int>();
        foreach(PlanetRequirement requirement in requirements)
        {
            requirement.CurrentlyHolding = 0;
            for(int i = 0; i < items.Count; i++)
            {
                if (requirement.ItemType.Type == items[i].Type && !usedIndices.Contains(i))
                {
                    // skip; possibly add them to the next requirement
                    if (requirement.CurrentlyHolding >= requirement.TotalRequired)
                    {
                        continue;
                    }

                    usedIndices.Add(i);
                    requirement.CurrentlyHolding++;
                }
            }

            if(requirement.CurrentlyHolding >= requirement.TotalRequired)
            {
                requirement.Complete();
            }
            else
            {
                requirement.InComplete();
            }
        }
    }

    // on trigger enter does not work in the case you drag immediately from wagon to planet; quick fix
    // TODO: Improve performance lol
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<CarriageItem>())
        {
            CarriageItem item = collision.GetComponent<CarriageItem>();
            if (item.carriageManager == null)
            {
                if (!items.Contains(item))
                {
                    items.Add(item);
                }
            }

            if (item.carriageManager != null)
            {
                if (items.Contains(item))
                {
                    items.Remove(item);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<CarriageItem>())
        {
            CarriageItem item = collision.GetComponent<CarriageItem>();
            if (item.carriageManager == null)
            {
                if (items.Contains(item))
                {
                    items.Remove(item);
                }
            }
        }
    }
}
