using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using TMPro;

public class CarriageManager : MonoBehaviour
{
    public List<CarriageItem> carriageItems { get; set; } = new List<CarriageItem>();

    private Vector3 offset;
    private bool isDragging = false;
    private Collider2D draggingCollider;
    [SerializeField] private float rotationSpeed = 5.0f;
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

        DragAndDrop();
        RotateDragging();

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

    private void OnDestroy()
    {
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

    private void RotateDragging()
    {
        if(!isDragging)
        {
            return;
        }

        if (Input.GetKey(KeyCode.A))
        {
            draggingCollider.gameObject.transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.unscaledDeltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            draggingCollider.gameObject.transform.Rotate(new Vector3(0, 0, -rotationSpeed) * Time.unscaledDeltaTime);
        }
    }

    private void DragAndDrop()
    {
        if (Input.GetMouseButton(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10, LayerMask.GetMask("Draggable"));

            if (hit.collider != null)
            {
                draggingCollider = hit.collider;
                offset = (Vector2)draggingCollider.transform.position - hit.point;
                isDragging = true;

                // set above other objects
                Transform t = draggingCollider.gameObject.transform;
                t.position = new Vector3(t.position.x, t.position.y, -1.0f);

                // stop able to drag others
                SetDraggableObjects(false, draggingCollider.GetComponent<CarriageItem>());
            }
        }
        else
        {
            if (draggingCollider != null)
            {
                // revert z-pos
                Transform t = draggingCollider.gameObject.transform;
                t.position = new Vector3(t.position.x, t.position.y, -0.01f);

                isDragging = false;
                draggingCollider = null;
                SetDraggableObjects(true, null);
            }
        }

        if (isDragging && draggingCollider != null)
        {
            // Update the object's position based on the mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            draggingCollider.transform.position = new Vector3(mousePosition.x, mousePosition.y, draggingCollider.transform.position.z);
        }
    }

    private void SetDraggableObjects(bool on, CarriageItem current)
    {
        foreach(CarriageItem item in carriageItems)
        {
            if(item == current)
            {
                continue;
            }

            if (on)
            {
                item.gameObject.layer = LayerMask.NameToLayer("Draggable");
            }
            else
            {
                item.gameObject.layer = LayerMask.NameToLayer("UnDraggable");
            }
        }
    }
}
