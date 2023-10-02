using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmogPirarticleController : MonoBehaviour
{
    [SerializeField] PirateMovement pirateMovement;
    [SerializeField] ParticleSystem leftSmog, rightSmog;

    [SerializeField] AnimationCurve smogIntensityOverTime;
    [SerializeField] float finalIntensityTime;

    [SerializeField] float speedRatio, smogRateRatio, smogAlphaRatio;

    float baseSpeed;
    float baseSmogRate;
    float baseSmogAlpha;
    Color baseColor;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        baseSpeed = leftSmog.main.startSpeed.constant;
        baseSmogRate = leftSmog.emission.rateOverTime.constant;
        baseSmogAlpha = leftSmog.main.startColor.color.a;
        baseColor = leftSmog.main.startColor.color;
    }

    // Update is called once per frame
    void Update()
    {
        // Can't do this on one line for some reason >:I
        var lMain = leftSmog.main;
        var rMain = rightSmog.main;
        var lEmission = leftSmog.emission;
        var rEmission = rightSmog.emission;

        if (pirateMovement.input.y > 0)
        {
            timer += Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, finalIntensityTime);
            float intensity = smogIntensityOverTime.Evaluate(timer / finalIntensityTime);
            
            lMain.startSpeed = baseSpeed + intensity * speedRatio;
            rMain.startSpeed = baseSpeed + intensity * speedRatio;
            lMain.startColor = new Color(baseColor.r, baseColor.g, baseColor.b, baseSmogAlpha + intensity * smogAlphaRatio);
            rMain.startColor = new Color(baseColor.r, baseColor.g, baseColor.b, baseSmogAlpha + intensity * smogAlphaRatio);
            lEmission.rateOverTime = baseSmogRate + intensity * smogRateRatio;
            rEmission.rateOverTime = baseSmogRate + intensity * smogRateRatio;
        }
        else
        {
            timer = 0;
            lMain.startSpeed = Mathf.Lerp(leftSmog.main.startSpeed.constant, baseSpeed, Time.deltaTime);
            rMain.startSpeed = Mathf.Lerp(rightSmog.main.startSpeed.constant, baseSpeed, Time.deltaTime);
            lMain.startColor = Color.Lerp(leftSmog.main.startColor.color, baseColor, Time.deltaTime);
            rMain.startColor = Color.Lerp(rightSmog.main.startColor.color, baseColor, Time.deltaTime);
            lEmission.rateOverTime = Mathf.Lerp(leftSmog.emission.rateOverTime.constant, baseSmogRate, Time.deltaTime);
            rEmission.rateOverTime = Mathf.Lerp(rightSmog.emission.rateOverTime.constant, baseSmogRate, Time.deltaTime);
        }
    }
}
