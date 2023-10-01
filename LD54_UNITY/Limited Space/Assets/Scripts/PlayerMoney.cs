using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlayerMoney : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyTmp;
    public int Money = 99;
    public bool isSpending = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        moneyTmp.text = Money.ToString();
    }

    public void ChangeMoney(int amount)
    {
        // unable to use money while animation
        if(isSpending == true)
        {
            return;
        }

        if(amount < 0)
        {
            // TODO negative money sound?
        }
        else
        {
            // TODO positive money sound
        }

        isSpending = true;
        DOTween.To(() => Money, x => Money = x, Money + amount, 1f)
            .OnUpdate(() => {

            })
            .OnComplete(() => {
                // This function will be called when the tween is complete.
                //Debug.Log("Tween Complete!");
                isSpending = false;
            });
    }
}
