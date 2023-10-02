using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingTextAnimator : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    [SerializeField] private float timeBetweenDots;
    [SerializeField] private int maxDots;

    int dotAmount;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        dotAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > timeBetweenDots)
        {
            timer = 0;
            dotAmount++;
            dotAmount %= maxDots;
            string loadingText = "Loading";

            for(int i = 0; i <= dotAmount; i++)
            {
                loadingText += ".";
            }

            text.SetText(loadingText);
        }
    }
}
