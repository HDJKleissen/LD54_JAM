using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> inventoryItems = new();
    [SerializeField] private List<Collider2D> edgeColliders = new();

    private Vector3 offset;
    [SerializeField] private bool isDragging = false;
    [SerializeField] private Collider2D draggingCollider;
    [SerializeField] private float rotationSpeed = 5.0f;
    private bool isInventoryOpen = false;
    private void Update()
    {
        DragAndDrop();
        RotateDragging();
    }

    public void CloseInventory()
    {

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
                SetDraggableObjects(false, draggingCollider.GetComponent<InventoryItem>());
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

    private void SetDraggableObjects(bool on, InventoryItem current)
    {
        foreach(InventoryItem item in inventoryItems)
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
