using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAnimation : MonoBehaviour
{
    [SerializeField] private float maxRandomRange = 0.1f;
    private float maxRange;
    float timingOffset;
    float maxFrequenceMultiplier = 1.5f;
    private CarriageItem carriageItem;

    [SerializeField] float maxRotationSpeed = 1.0f;
    private float rotationSpeed;
    private void Awake()
    {
        carriageItem = GetComponent<CarriageItem>();
    }
    void Start()
    {
        maxRange = maxRandomRange + Random.Range(0, 0.1f);
        timingOffset = Random.value * (Mathf.PI / 2);
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        // dont move when in a wagon
        if(carriageItem && carriageItem.carriageManager != null)
        {
            return;
        }

        transform.Translate(new Vector3(Mathf.Sin(Time.unscaledTime * Random.Range(1, maxFrequenceMultiplier) + timingOffset),
                                        Mathf.Sin(Time.unscaledTime * Random.Range(1, maxFrequenceMultiplier) + timingOffset),
                                        0)
                                        * Time.unscaledDeltaTime * maxRange);

        transform.Rotate(new Vector3(0, 0, 1), Time.unscaledDeltaTime * rotationSpeed);
    }

}
