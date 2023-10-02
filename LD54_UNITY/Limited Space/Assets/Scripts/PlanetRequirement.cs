using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PlanetRequirement : MonoBehaviour
{
    [SerializeField] public int TotalRequired = 3;
    [SerializeField] public ItemType ItemType;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private TextMeshProUGUI requiredAmountTmp;
    [SerializeField] private Image requiredImage;
    [SerializeField] private Image background;
    public int CurrentlyHolding;
    public int Reward = 25;
    public bool IsComplete { get; private set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        DisplayRequirements();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Complete()
    {
        background.color = Color.green;
        IsComplete = true;
    }

    public void InComplete()
    {
        background.color = Color.white;
        IsComplete = false;
    }

    internal void Randomize(ItemType newCarriageItemType, Sprite newCarriageItemSprite)
    {
        ItemType = newCarriageItemType;
        itemSprite = newCarriageItemSprite;
        TotalRequired = UnityEngine.Random.Range(1, 8);
        Reward = TotalRequired + 35;
        InComplete();

        DisplayRequirements();
    }

    public void DisplayRequirements()
    {
        requiredAmountTmp.text = TotalRequired.ToString();
        requiredImage.sprite = itemSprite;
    }
}
