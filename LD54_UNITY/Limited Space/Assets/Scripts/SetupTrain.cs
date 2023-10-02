using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SetupTrain : MonoBehaviour
{
    [SerializeField] private int _containerAmount;
    [SerializeField] private Rigidbody2D _trainCabin;
    [SerializeField] private GameObject _containerPrefab;
    [SerializeField] private GameObject _connectorPrefab;
    [SerializeField] private float yOffset;
    [SerializeField] bool allowContainerSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void OnValidate()
    {
        if (allowContainerSpawn)
        {
            SetContainers(_containerAmount);
            allowContainerSpawn = false;
        }
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

        Rigidbody2D previousContainer = _trainCabin;
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
}
