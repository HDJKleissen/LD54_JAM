using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class SetupTrain : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _trainCabin;
    [SerializeField] private GameObject _containerPrefab;
    [SerializeField] private GameObject _connectorPrefab;
    [SerializeField] private float yOffset;
    [SerializeField] bool allowContainerSpawn;

    List<Container> containers = new List<Container>();
    Rigidbody2D previousContainer;
    PlayerMoney money;
    // Start is called before the first frame update
    void Start()
    {
        money = FindObjectOfType<PlayerMoney>();
        containers = new List<Container>(GetComponentsInChildren<Container>());
        if (containers.Count == 0)
        {
            previousContainer = _trainCabin;
        }
        else
        {
            previousContainer = containers[containers.Count - 1].GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        int aliveContainers = containers.Count;
        foreach(Container container in containers)
        {
            if(container.health < 0)
            {
                aliveContainers--;
            }
        }

        if(aliveContainers == 0 && money.Money <= 0)
        {
            FindObjectOfType<MenuButtons>().OpenGameOverScreen();
            PlayerMovement mov = FindObjectOfType<PlayerMovement>();
            mov.movementSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            mov.enabled = false;
        }
    }

    
    void OnValidate()
    {
        if (allowContainerSpawn)
        {
            containers = new List<Container>(GetComponentsInChildren<Container>());
            if(containers.Count == 0)
            {
                previousContainer = _trainCabin;
            }
            AddContainer();
            allowContainerSpawn = false;
        }
    }

    public void AddContainer()
    {
        int containerNum = containers.Count;
        GameObject container = Instantiate(_containerPrefab);
        container.name = "Container " + (containerNum + 1);
        container.transform.parent = transform;
        container.transform.localPosition = previousContainer.transform.localPosition + new Vector3(0, yOffset, 0);
        containers.Add(container.GetComponent<Container>());
        HingeJoint2D hingeJoint = container.GetComponent<HingeJoint2D>();
        DistanceJoint2D distanceJoint = container.GetComponent<DistanceJoint2D>();
        hingeJoint.connectedBody = previousContainer;
        distanceJoint.connectedBody = previousContainer;

        GameObject connector = Instantiate(_connectorPrefab);
        connector.transform.parent = transform;
        connector.GetComponent<ContainerConnector>().SetConnection(previousContainer.transform, container.transform);

        previousContainer = container.GetComponent<Rigidbody2D>();
    }

    public void SetContainers(int amount) // todo change to use container data
    {
        //foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        //{
        //    if (child != transform && (child.GetComponent<Container>() != null || child.GetComponent<ContainerConnector>() != null))
        //    {
        //        DestroyImmediate(child.gameObject);
        //    }
        //}

        for(int i = 0; i < amount; i++)
        {
            GameObject container = Instantiate(_containerPrefab);
            container.name = "Container " + (i + 1);
            container.transform.parent = transform;
            container.transform.localPosition = new Vector3(0, yOffset * (i+1), 0);
            HingeJoint2D hingeJoint = container.GetComponent<HingeJoint2D>();
            DistanceJoint2D distanceJoint = container.GetComponent<DistanceJoint2D>();
            hingeJoint.connectedBody = previousContainer;
            distanceJoint.connectedBody = previousContainer;

            GameObject connector = Instantiate(_connectorPrefab);
            connector.transform.parent = transform;
            connector.GetComponent<ContainerConnector>().SetConnection(previousContainer.transform, container.transform);

            previousContainer = container.GetComponent<Rigidbody2D>();
        }
    }

    internal void RepairTrain()
    {
        _trainCabin.GetComponent<PlayerMovement>().RepairFull();
        foreach (Container container in containers)
        {
            container.RepairFull();
        }
    }
}
