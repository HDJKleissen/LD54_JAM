using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    PlayerMoney money;
    PlayerGas gas;
    PlayerMovement movement;
    SetupTrain setupTrain;
    [SerializeField] Transform canvas;
    Planet planet;
    ItemSpawner planetItemSpawner;

    Vector3 baseScale;

    // Start is called before the first frame update
    void Awake()
    {
        money = FindObjectOfType<PlayerMoney>();
        gas = FindObjectOfType<PlayerGas>();
        movement = FindObjectOfType<PlayerMovement>();
        setupTrain = FindObjectOfType<SetupTrain>();
        planet = transform.parent.GetComponentInChildren<Planet>();
        planetItemSpawner = planet.GetComponent<ItemSpawner>();
        baseScale = canvas.localScale;
        CloseShop();
    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            OpenShop();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CloseShop();
        }
    }

    private void OpenShop()
    {
        //SFX: Open shop
        canvas.gameObject.SetActive(true);

        // Create a new DOTween sequence
        Sequence mySequence = DOTween.Sequence();

        // Add a scale animation from 1 to 0 over a duration of 1 second
        mySequence.Append(canvas.transform.DOScale(baseScale, 0.5f).SetEase(Ease.OutExpo));

        // Play the sequence
        mySequence.Play();
    }

    private void CloseShop()
    {
        // Create a new DOTween sequence
        Sequence mySequence = DOTween.Sequence();

        // Add a scale animation from 1 to 0 over a duration of 1 second
        mySequence.Append(canvas.transform.DOScale(new Vector3(baseScale.x, baseScale.y * 0.15f, baseScale.z), 0.5f).SetEase(Ease.OutExpo));

        // Add an action to disable the object when the animation is finished
        mySequence.OnComplete(() =>
        {
            canvas.gameObject.SetActive(false);
        });

        // Play the sequence
        mySequence.Play();
        //SFX: Close shop
    }

    public void BuyExtraWagon(ShopItem item)
    {
        if (Buy(item.price))
        {
            //SFX: Wrench sound
            Debug.LogWarning("ADding Extra Wagon!");
            setupTrain.AddContainer();
        }
    }

    public void BuyExtraSpeed(ShopItem item)
    {
        if (Buy(item.price))
        {
            //SFX: Wrench sound
            movement.IncreaseMaxMovementSpeed(item.amount);
            Debug.LogWarning("ADding Extra Speed!");
        }
    }

    public void BuyExtraBrakeSpeed(ShopItem item)
    {
        if (Buy(item.price))
        {
            //SFX: Wrench sound
            movement.IncreaseBreakSpeed(item.amount);
            Debug.LogWarning("ADding Better Brakes!");
        }
    }

    public void FillGas(ShopItem item)
    {
        if (Buy(item.price))
        {
            //SFX: Fill Gas
            gas.AddGasPerPercentage(item.amount);
            Debug.LogWarning("Filling Gas!!");
        }
    }

    public bool Buy(int price)
    {
        if (money.Money >= price && !money.isSpending)
        {
            money.ChangeMoney(-price);
            //SFX: succesful buy sound
            return true;
        }

        //SFX: unsuccesful buy sound
        return false;
    }

    public void BuyCarriageItems(ShopItem item)
    {
        if (Buy(item.price))
        {
            planetItemSpawner.SpawnItems(1, item.carriageItem);
            Debug.LogWarning("Spawn Items!!");
        }
    }
}