using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPlayerRotation : MonoBehaviour
{
    public Transform playerImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = playerImage.rotation;   
    }
}
