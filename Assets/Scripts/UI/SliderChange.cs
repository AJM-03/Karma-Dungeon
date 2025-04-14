using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderChange : MonoBehaviour
{
    public Slider slider;
    public PlayerMovement playerMovement;
    public Image bar;
    public Sprite none;
    public Sprite good;
    public Sprite bad;
    public GameObject anim;

    public void ChangeValue(float value)
    {
        slider.value = value;
    }

    private void Update()
    {
        if (playerMovement != null && playerMovement.karma != null)
        {
            if (playerMovement.karma.karma > 55)
            {
                bar.sprite = good;
            }
            else if (playerMovement.karma.karma < 45)
            {
                bar.sprite = bad;
            }
            else
            {
                bar.sprite = none;
            }

            if (playerMovement.karma.karma == 100 || playerMovement.karma.karma == 0)
            {
                anim.SetActive(true);
            }
            else
                anim.SetActive(false);
        }
    }
}
