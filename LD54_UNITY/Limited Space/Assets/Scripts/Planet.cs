using DG.Tweening;
using System;
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
    CanvasGroup canvas;
    bool playedBark;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        requirements = requirementsContainer.GetComponentsInChildren<PlanetRequirement>().ToList();
    }

    internal void AddRandomRequirement(GameObject requirementPrefab, ItemType newRequirementItemPrefab, Sprite newRequirementItemSprite)
    {
        GameObject reqGO = Instantiate(requirementPrefab);

        reqGO.transform.SetParent(requirementsContainer);
        PlanetRequirement pr = reqGO.GetComponent<PlanetRequirement>();

        pr.Randomize(newRequirementItemPrefab, newRequirementItemSprite);
        requirements.Add(pr);
        ShowObjectiveCanvas();
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = requirementsContainer.parent.parent.GetComponent<CanvasGroup>();
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


    void CompletePlanet()
    {
        //SFX: play complete planet audio

        List<CarriageItem> totalToDestroy = new List<CarriageItem>();

        foreach (PlanetRequirement requirement in requirements)
        {
            List<CarriageItem> toDestroy = new List<CarriageItem>();
            List<CarriageItem> requirementItems = new List<CarriageItem>(items.Where(item => item.Type == requirement.ItemType));

            foreach (CarriageItem requiredItem in requirementItems)
            {
                if (toDestroy.Count < requirement.TotalRequired)
                {
                    toDestroy.Add(requiredItem);
                    totalToDestroy.Add(requiredItem);
                }
            }
        }

        Sequence mySequence = DOTween.Sequence();
        for (int i = 0; i < totalToDestroy.Count; i++)
        {
            mySequence.Join(totalToDestroy[i].transform.DOMove(
                transform.position + new Vector3(
                    UnityEngine.Random.Range(-1, 1),
                    UnityEngine.Random.Range(-1, 1),
                    0),
                UnityEngine.Random.Range(0.5f, 0.7f),
                false)
                .SetEase(Ease.InQuad));
        }

        int moneyAdded = 0;
        foreach (PlanetRequirement requirement in requirements)
        {
            moneyAdded += requirement.Reward;
        }

        mySequence.OnComplete(() => {
            int toDestroyItemAmount = totalToDestroy.Count;

            for (int i = 0; i < toDestroyItemAmount; i++)
            {
                items.Remove(totalToDestroy[i]);
                Destroy(totalToDestroy[i].gameObject);
            }

            HideObjectiveCanvas();

            int toDestroyRequirementsAmount = requirements.Count;
            for (int i = 0; i < toDestroyItemAmount; i++)
            {
                Destroy(requirements[i].gameObject);
            }
            requirements.Clear();

            FMODUnity.RuntimeManager.PlayOneShot("event:/Delivery");
            FindObjectOfType<PlayerMoney>().ChangeMoney(moneyAdded);
            IsComplete = false;
            playedBark = false;
        });
    }

    // fade out and disable
    private void HideObjectiveCanvas()
    {
        DOTween.To(() => canvas.alpha, x => canvas.alpha = x, 0, 1f)
             .OnComplete(() => {
                 // This function will be called when the tween is complete.
                 //Debug.Log("Tween Complete!");
                 requirementsContainer.parent.parent.gameObject.SetActive(false);
             });
    }

    // fade in and enable
    private void ShowObjectiveCanvas()
    {
        requirementsContainer.parent.parent.gameObject.SetActive(true);
        DOTween.To(() => canvas.alpha, x => canvas.alpha = x, 1f, 1f);
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
        
        if(requirements.Count == 0)
        {
            complete = false;
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
                if (requirement.ItemType == items[i].Type && !usedIndices.Contains(i) && !items[i].BeingDragged)
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
            if (requirement.CurrentlyHolding >= requirement.TotalRequired)
            {
                requirement.Complete();
            }
            else
            {
                requirement.InComplete();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && requirements.Count > 0 && !playedBark)
        {
            playedBark = true;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Quest Bark");
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
