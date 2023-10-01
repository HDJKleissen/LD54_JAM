using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [field: SerializeField] public Transform WorldItemsContainer { get; private set; }

    public List<CarriageManager> openCarriages = new List<CarriageManager>();
    public int maxOpenCarriages = 2;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckMouseClick();
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
                Debug.Log("Im over 'This'");
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

    public void CloseCarriage(CarriageManager carriage)
    {
        carriage.CloseCarriage();
    }

    public void OpenCarriage(CarriageManager carriage)
    {
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
