using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    Transform player;
    public Transform ItemContainer;
    public List<CarriageItem> items = new List<CarriageItem>();
    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // on trigger enter does not work in the case you drag immediately from wagon to planet; quick fix
    // TODO: Improve performance lol
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<CarriageItem>())
        {
            CarriageItem item = collision.GetComponent<CarriageItem>();
            if (item.carriageManager == null)
            {
                if (!items.Contains(item))
                {
                    items.Add(item);
                }
            }

            if (item.carriageManager != null)
            {
                if (items.Contains(item))
                {
                    items.Remove(item);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<CarriageItem>())
        {
            CarriageItem item = collision.GetComponent<CarriageItem>();
            if (item.carriageManager == null)
            {
                if (items.Contains(item))
                {
                    items.Remove(item);
                }
            }
        }
    }
}
