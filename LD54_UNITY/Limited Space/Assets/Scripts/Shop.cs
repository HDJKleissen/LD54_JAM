using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    PlayerMoney money;
    PlayerGas gas;
    PlayerMovement movement;
    Collider2D playerCollider;
    [SerializeField] Transform canvas;
    Collider2D planetCollider;
    private bool isShopOpen = false;
    Planet planet;
    // Start is called before the first frame update
    void Awake()
    {
        money = FindObjectOfType<PlayerMoney>();
        gas = FindObjectOfType<PlayerGas>();
        movement = FindObjectOfType<PlayerMovement>();
        playerCollider = movement.GetComponentInChildren<Collider2D>();
        planet = transform.parent.GetComponentInChildren<Planet>();
        planetCollider = planet.GetComponentInChildren<Collider2D>();
        isShopOpen = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckOpenShop();
    }

    private void CheckOpenShop()
    {
        if (planetCollider.bounds.Intersects(playerCollider.bounds))
        {
            if (!isShopOpen)
            {
                OpenShop();
            }
        }
        else
        {
            if (isShopOpen)
            {
                CloseShop();
            }
        }
    }

    //TODO animate open/close
    private void OpenShop()
    {
        //TODO AUDIO open shop
        isShopOpen = true;
        canvas.gameObject.SetActive(true);
    }

    private void CloseShop()
    {
        //TODO AUDIO close shop
        isShopOpen = false;
        canvas.gameObject.SetActive(false);
    }

    public void BuyExtraWagon(ShopItem item)
    {
        if (Buy(item.price))
        {
            //TODO Add extra wagon
            Debug.LogWarning("ADding Extra Wagon!");
        }
    }

    public void BuyExtraSpeed(ShopItem item)
    {
        if (Buy(item.price))
        {
            movement.IncreaseMaxMovementSpeed(item.amount);
            Debug.LogWarning("ADding Extra Speed!");
        }
    }

    public void BuyExtraBrakeSpeed(ShopItem item)
    {
        if (Buy(item.price))
        {
            movement.IncreaseBreakSpeed(item.amount);
            Debug.LogWarning("ADding Better Brakes!");
        }
    }

    public void FillGas(ShopItem item)
    {
        if (Buy(item.price))
        {
            //TODO Fill Gas
            gas.AddGasPerPercentage(item.amount);
            Debug.LogWarning("Filling Gas!!");
        }
    }

    public bool Buy(int price)
    {
        if (money.Money >= price && !money.isSpending)
        {
            money.ChangeMoney(-price);
            //TODO succesful buy sound
            return true;
        }

        //TODO unsuccesful buy sound
        return false;
    }

    public void BuyCarriageItems(ShopItem item)
    {
        if (Buy(item.price))
        {
            //TODO Spawn Items
            Debug.LogWarning("Spawn Items!!");
        }
    }
}
