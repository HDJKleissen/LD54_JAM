using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlanetRequirement : MonoBehaviour
{
    [SerializeField] public int TotalRequired = 3;
    [SerializeField] public CarriageItem ItemType;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private TextMeshProUGUI requiredAmountTmp;
    [SerializeField] private Image requiredImage;
    [SerializeField] private Image background;
    public int CurrentlyHolding;
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

    public void DisplayRequirements()
    {
        requiredAmountTmp.text = TotalRequired.ToString();
        requiredImage.sprite = itemSprite;
    }
}
