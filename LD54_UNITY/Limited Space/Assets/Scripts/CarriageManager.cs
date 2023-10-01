using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using TMPro;

public class CarriageManager : MonoBehaviour
{
    public List<CarriageItem> carriageItems { get; set; } = new List<CarriageItem>();

    [field: SerializeField] public bool IsCarriageOpen { get; private set; } = false;
    private Vector3 smallScale = new Vector3(1.0f, 0.15f, 1.0f);

    [SerializeField] private BoxCollider2D carriage;
    private float totalOccupy = 0.0f;
    private float percentageOccupy = 0.0f;
    private float carriageSize;
    [SerializeField] private TextMeshProUGUI accuracyTmp;
    [SerializeField] private Transform itemContainer;

    InventoryManager inventoryManager;
    [field: SerializeField] public Transform CloseButtonCanvas { get; private set; }
    public List<Collider2D> OutsideColliders = new List<Collider2D>();

    public bool IsOpenLeft = false;
    private void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        transform.localScale = smallScale;
        carriageItems.Clear();
        carriageSize = carriage.GetComponent<BoxCollider2D>().size.x * carriage.GetComponent<BoxCollider2D>().size.y;

    }

    private void Update()
    {
        if (!IsCarriageOpen)
        {
            return;
        }


        CalculateFitness();
        RotateUIToPlayer();
    }

    public void AddCarriageItem(CarriageItem carriageItem)
    {
        carriageItems.Add(carriageItem);
        carriageItem.transform.SetParent(itemContainer, true);
        carriageItem.carriageManager = this;
    }

    public void RemoveCarriageItem(CarriageItem carriageItem)
    {
        carriageItem.transform.SetParent(inventoryManager.WorldItemsContainer, true);
        carriageItems.Remove(carriageItem);
        carriageItem.carriageManager = null;
    }

    private void OnEnable()
    {
        IsCarriageOpen = false;
        OpenCarriage();
    }

    private void CalculateTotalOccupy()
    {
        totalOccupy = 0;
        foreach(CarriageItem carriage in carriageItems)
        {
            totalOccupy += carriage.ItemSize;
        }
    }

    private void CalculateFitness()
    {
        CalculateTotalOccupy();
        percentageOccupy = totalOccupy / carriageSize * 100.0f;
        Debug.Log($"Carraige size = {carriageSize} totalOcc = {totalOccupy}");
        accuracyTmp.text = $"{percentageOccupy.ToString("0")}%";
    }

    private void RotateUIToPlayer()
    {
        accuracyTmp.transform.rotation = Quaternion.identity;
        CloseButtonCanvas.rotation = Quaternion.identity;
    }

    public void OpenCarriage()
    {
        gameObject.SetActive(true);

        // Create a new DOTween sequence
        Sequence mySequence = DOTween.Sequence();

        // Add a scale animation from 1 to 0 over a duration of 1 second
        mySequence.Append(transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo));

        // Play the sequence
        mySequence.Play();

        mySequence.OnComplete(() =>
        {
            IsCarriageOpen = true;
        });
    }

    public void TryCloseCarriage()
    {
        if(carriageItems.All(item => item.IsFitCorrectly))
        {
            inventoryManager.CloseCarriage(this);
            // TODO AUDIO: play fit SFX
        }
        else
        {
            // TODO AUDIO: play not fit SFX
        }
    }

    public void CloseCarriage()
    {
        IsCarriageOpen = false;

        // Create a new DOTween sequence
        Sequence mySequence = DOTween.Sequence();

        // Add a scale animation from 1 to 0 over a duration of 1 second
        mySequence.Append(transform.DOScale(new Vector3(1.0f, 0.15f, 1.0f), 0.5f).SetEase(Ease.OutExpo));

        // Add an action to disable the object when the animation is finished
        mySequence.OnComplete(() =>
        {
            gameObject.SetActive(false);
            inventoryManager.RemoveCarriage(this);
        });

        // Play the sequence
        mySequence.Play();
    }

}
