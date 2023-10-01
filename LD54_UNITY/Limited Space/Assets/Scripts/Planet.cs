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
    public int TotalMoneyReward = 25;
    [SerializeField] Transform requirementsContainer;
    public bool IsComplete { get; private set; } = false;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        requirements = requirementsContainer.GetComponentsInChildren<PlanetRequirement>().ToList();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // nice performance lol, the adding and removing of objects is too shitty so have to do it like dies
        if (!IsComplete)
        {
            // if items are on the planet
            CheckRequirements();
            CheckIsPlanetComplete();

            if (IsComplete)
            {
                Debug.LogWarning($"PLANET COMPLETE {items.Count}");
                CompletePlanet();
            }
        }
    }

    void DeleteItems()
    {
        int totalDelete = items.Count;
        for (int i = 0; i < totalDelete; i++)
        {
            Destroy(items[0].gameObject);
        }

        items.Clear();
    }

    void CompletePlanet()
    {
        //TODO play complete planet audio
        Sequence mySequence = DOTween.Sequence();
        for (int i = 0; i < items.Count; i++)
        {
            mySequence.Join(items[i].transform.DOMove(  this.transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1,1), 0),
                                                        Random.Range(0.5f, 0.7f),
                                                        false)
                                                        .SetEase(Ease.InQuad));
        }

        mySequence.OnComplete(() =>
        {
            DeleteItems();
            FindObjectOfType<PlayerMoney>().ChangeMoney(TotalMoneyReward);
            HideObjectiveCanvas();
        });
    }

    // fade out and disable
    private void HideObjectiveCanvas()
    {
        CanvasGroup canvas = requirementsContainer.parent.parent.GetComponent<CanvasGroup>();
        DOTween.To(() => canvas.alpha, x => canvas.alpha = x, 0, 1f)
             .OnUpdate(() => {

             })
             .OnComplete(() => {
                 // This function will be called when the tween is complete.
                 //Debug.Log("Tween Complete!");
                requirementsContainer.parent.parent.gameObject.SetActive(false);
             });
    }

    public bool CheckIsPlanetComplete()
    {
        bool complete = true;
        foreach(PlanetRequirement requirement in requirements)
        {
            if (!requirement.IsComplete)
            {
                complete = false;
            }
        }

        IsComplete = complete;
        return IsComplete;
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
