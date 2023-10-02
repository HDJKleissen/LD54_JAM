using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerConnector : MonoBehaviour
{
    [SerializeField] private Transform _frontContainer;
    [SerializeField] private Transform _backContainer;



    bool isValid => _frontContainer != null && _backContainer != null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isValid)
        {
            UpdateTransform();
        }
    }

    void UpdateTransform()
    {
        transform.rotation = Quaternion.Lerp(_frontContainer.rotation, _backContainer.rotation, 0.5f);
        transform.position = (_frontContainer.position + _backContainer.position) / 2;
    }

    public void SetConnection(Transform front, Transform back)
    {
        _frontContainer = front;
        _backContainer = back;
        UpdateTransform();
    }
}