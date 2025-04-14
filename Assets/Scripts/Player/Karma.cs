using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Karma : MonoBehaviour
{
    public float karma;
    public SliderChange karmaBar;

    public void KarmaAdd(float amount)
    {
        //Debug.Log(amount);
        karma = karma + amount;
        karma = Mathf.Clamp(karma, 0, 100);
        karmaBar.ChangeValue(karma);

    }

    public void KarmaReset()
    {
        //Debug.Log(amount);
        karma = 50;;
        karmaBar.ChangeValue(karma);
    }
}
