using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    public void PlayConfirmSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIConfirm");
    }

    public void PlayBackSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIBack");
    }
}