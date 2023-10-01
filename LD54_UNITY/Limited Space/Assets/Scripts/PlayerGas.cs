using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGas : MonoBehaviour
{
    [SerializeField] Slider gasSlider;
    [SerializeField] float gasAmount = 100.0f;
    [SerializeField] float maxGas = 100.0f;
    [SerializeField] float useGasMultiplier = 1.0f;

    private bool isRanOut = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gasSlider.value = gasAmount / maxGas;

        if(gasAmount < 0 && !isRanOut)
        {
            isRanOut = true;
            Debug.LogWarning("GAME OVER!!");
        }
    }

    public void ReduceGas(float amount)
    {
        gasAmount -= amount * useGasMultiplier * Time.deltaTime;
    }
}
