using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriageItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    public bool IsFitCorrectly { get; private set; } = true;

    public float ItemSize { get; private set; }

    private InventoryManager inventoryManager;
    public CarriageManager carriageManager { get; set; }

    bool isClippingOutside = false;

    public bool BeingDragged = false;

    public ItemType Type;

    void Start()
    {
        ItemSize = CalculateArea();
    }

    // Update is called once per frame
    void Update()
    {
        if(carriageManager == null)
        {
            sprite.sortingOrder = 0;
            sprite.color = Color.white;
        }
        else if (carriageManager)
        {
            sprite.sortingOrder = 1;
            sprite.color = Color.white;
        }

        if(carriageManager && !IsFitCorrectly)
        {
            sprite.color = Color.red;
        }
    }

    // flip between inside and outside of the carriage
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!BeingDragged)
        {
            return;
        }
        if (carriageManager && !carriageManager.IsCarriageOpen)
        {
            return;
        }

        if (collision.tag == "CarriageItem" && carriageManager == collision.GetComponent<CarriageItem>().carriageManager)
        {
            IsFitCorrectly = false;
        }

        if(carriageManager && carriageManager.OutsideColliders.Exists(item => item == collision)) 
        {
            isClippingOutside = true;
            if (carriageManager && carriageManager.carriageItems.Contains(this))
            {
                if (Type == ItemType.Tumbleweed)
                {
                    GetComponent<Rigidbody2D>().isKinematic = false;
                }
                carriageManager.RemoveCarriageItem(this);
            }
        }

        if (collision.tag == "CarriageContainer" && carriageManager == null && collision.GetComponent<CarriageManager>().IsCarriageOpen)
        {
            if (collision.GetComponent<CarriageManager>() && !collision.GetComponent<CarriageManager>().carriageItems.Contains(this))
            {
                if (!isClippingOutside)
                {
                    if(Type == ItemType.Tumbleweed)
                    {
                        Rigidbody2D itemRB = GetComponent<Rigidbody2D>();
                        itemRB.isKinematic = true;
                        itemRB.velocity = Vector2.zero;
                    }
                    collision.GetComponent<CarriageManager>().AddCarriageItem(this);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (carriageManager && !carriageManager.IsCarriageOpen)
        {
            return;
        }

        if (collision.tag == "CarriageItem" && carriageManager == collision.GetComponent<CarriageItem>().carriageManager)
        {
            IsFitCorrectly = true;
        }

        if (collision.tag == "OutsideCarriage")
        {
            isClippingOutside = false;
        }
    }

    private float CalculateArea()
    {
        if (GetComponent<PolygonCollider2D>()){
            return CalculatePolygonArea();
        }
        else
        {
            return CalculateRectangleArea();
        }
    }

    private float CalculateRectangleArea()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        return collider.size.x * transform.localScale.x * collider.size.y * transform.localScale.y;
    }

    private float CalculatePolygonArea()
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        Vector2[] points = collider.GetPath(0); // Get the points of the collider's path

        int pointCount = points.Length;

        if (pointCount < 3)
        {
            Debug.LogError("Polygon must have at least 3 points.");
            return 0f;
        }

        float area = 0f;

        for (int i = 0; i < pointCount; i++)
        {
            Vector2 currentPoint = points[i];
            Vector2 nextPoint = points[(i + 1) % pointCount]; // Wrap around to the first point

            area += (currentPoint.x * nextPoint.y - nextPoint.x * currentPoint.y);
        }

        area /= 2f;
        return Mathf.Abs(area);
    }
}

public enum ItemType
{
    Cactus,
    Crate,
    Wood,
    Briefcase,
    Tumbleweed,
}