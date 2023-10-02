using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [field: SerializeField] public Transform WorldItemsContainer { get; private set; }

    public List<CarriageManager> openCarriages = new List<CarriageManager>();
    public int maxOpenCarriages = 2;

    //moving items
    private Vector3 offset;
    private bool isDragging = false;
    private Collider2D draggingCollider;
    private CarriageItem draggingItem;
    [SerializeField] private float rotationSpeed = 5.0f;

    bool moving =true;
    // Update is called once per frame
    void Update()
    {
        CheckMouseClick();
        DragAndDrop();
        RotateDragging();
    }

    private void DragAndDrop()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10, LayerMask.GetMask("Draggable"));

            if (hit.collider != null)
            {
                draggingCollider = hit.collider;
                draggingItem = hit.collider.GetComponent<CarriageItem>();
                draggingItem.BeingDragged = true;
                offset = (Vector2)draggingCollider.transform.position - hit.point;
                isDragging = true;

                FMODUnity.RuntimeManager.PlayOneShot("event:/Pickup");

                // set above other objects
                Transform t = draggingCollider.gameObject.transform;
                t.position = new Vector3(t.position.x, t.position.y, -1.0f);
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (draggingCollider != null)
            {
                if(draggingItem != null)
                {
                    draggingItem.BeingDragged = false;
                    draggingItem = null;
                }

                // revert z-pos
                Transform t = draggingCollider.gameObject.transform;
                t.position = new Vector3(t.position.x, t.position.y, 0);
                isDragging = false;
                draggingCollider = null;
            }
        }

        if (isDragging && draggingCollider != null)
        {
            // Update the object's position based on the mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            draggingCollider.transform.position = new Vector3(mousePosition.x, mousePosition.y, draggingCollider.transform.position.z);
        }
    }

    internal void SetTrainIsMoving(bool moving)
    {
        this.moving = moving;
        if (moving) {
            if (TrainCanBeClosed())
            {
                foreach(CarriageManager carriage in openCarriages)
                {
                    carriage.CloseCarriage();
                }
            }
        }
    }
    
    internal bool TrainCanBeClosed()
    {
        bool canClose = true;
        foreach (CarriageManager carriage in openCarriages)
        {
            if(!carriage.carriageItems.All(item => item.IsFitCorrectly))
            {
                canClose = false;
            }
        }

        return canClose;
    }

    private void RotateDragging()
    {
        if (!isDragging)
        {
            return;
        }

        if(draggingCollider == null)
        {
            return;
        }

        if (Input.GetKey(KeyCode.A))
        {
            draggingCollider.gameObject.transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            draggingCollider.gameObject.transform.Rotate(new Vector3(0, 0, -rotationSpeed) * Time.deltaTime);
        }
    }

    void CheckMouseClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10);

            if (hit && hit.collider.GetComponent<Container>())
            {
                hit.collider.GetComponent<Container>().OnClick(this);
            }
        }
    }

    public void SetCarriageState(CarriageManager carriage)
    {
        if (carriage.gameObject.activeSelf)
        {
            carriage.CloseCarriage();
        }
        else
        {
            OpenCarriage(carriage);
        }
    }


    public void OpenCarriage(CarriageManager carriage)
    {
        if (moving)
        {
            return;
        }
        if (!carriage.gameObject.activeSelf)
        {
            if (openCarriages.Count >= maxOpenCarriages)
            {
                return;
            }

            //flip compared to current open
            if (openCarriages.Count == 1 && openCarriages[0].IsOpenLeft)
            {
                OpenRight(carriage);
            }
            else
            {
                OpenLeft(carriage);
            }

            openCarriages.Add(carriage);
            carriage.OpenCarriage();
        }
    }

    public void RemoveCarriage(CarriageManager carriageManager)
    {
        openCarriages.Remove(carriageManager);
    }

    //flip it to the other side
    private void OpenLeft(CarriageManager carriage)
    {
        carriage.IsOpenLeft = true;
        carriage.gameObject.transform.localPosition = new Vector3(3.5f, carriage.gameObject.transform.localPosition.y, carriage.gameObject.transform.localPosition.z);
        carriage.CloseButtonCanvas.transform.localPosition = new Vector3(carriage.CloseButtonCanvas.transform.localPosition.x, -2.64f, carriage.CloseButtonCanvas.transform.localPosition.z);
    }

    private void OpenRight(CarriageManager carriage)
    {
        carriage.IsOpenLeft = false;
        carriage.gameObject.transform.localPosition = new Vector3(-3.5f, carriage.gameObject.transform.localPosition.y, carriage.gameObject.transform.localPosition.z);
        carriage.CloseButtonCanvas.transform.localPosition = new Vector3(carriage.CloseButtonCanvas.transform.localPosition.x, 2.64f, carriage.CloseButtonCanvas.transform.localPosition.z);

    }
}
