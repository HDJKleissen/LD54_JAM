using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public int price;
    public int amount;

    [Header("Buying Items settings (optional)")]
    public CarriageItem carriageItem;
    [SerializeField] Sprite carriageItemSprite;

    [Header("References (optional)")]
    [SerializeField] TextMeshProUGUI priceTmp;
    [SerializeField] Image carriageItemImage;
    [SerializeField] TextMeshProUGUI buttonText;

    public enum Type
    {
        Gas,
        Items,
        Other,
    }

    public Type type;
    private void Awake()
    {
        carriageItemImage.sprite = carriageItemSprite;
        priceTmp.text = price.ToString();

        if (type == Type.Items)
        {
            buttonText.text = amount.ToString();

            price = Random.Range(1, 10);
            amount = Random.Range(1, 15);
        }

        if(type == Type.Gas)
        {
            buttonText.text = amount.ToString() + "%";
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
