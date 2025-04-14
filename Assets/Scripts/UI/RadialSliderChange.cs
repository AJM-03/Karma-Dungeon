using UnityEngine;
using UnityEngine.UI;

public class RadialSliderChange : MonoBehaviour
{
    public Transform bar;
    public float currentAmount;
    public float speed;
    public PlayerMovement playerMovement;
    public Color white;
    public Color green;
    public Color red;
    public Color lightBlue;
    public Color blue;

    private void Start()
    {
        currentAmount = 0f;
    }

    private void Update()
    {
        if(currentAmount < 100)
        {
            currentAmount += speed * Time.deltaTime;

            if (playerMovement.form == "Soul")
                if (playerMovement.karma.karma == 100 || playerMovement.karma.karma == 0)
                    currentAmount += speed * Time.deltaTime;
        }

        else
        {
            if (playerMovement.form == "Soul")
                playerMovement.Rebirth();
            else if (playerMovement.form == "Neutral")
                playerMovement.Return();

            currentAmount = 0f;
            gameObject.SetActive(false);
        }

        bar.GetComponent<Image>().fillAmount = currentAmount / 100;

        if (playerMovement.form == "Soul")
        {
            if (playerMovement.karma.karma > 55)
            {
                bar.gameObject.GetComponent<Image>().color = lightBlue;
            }
            else if (playerMovement.karma.karma < 45)
            {
                bar.gameObject.GetComponent<Image>().color = red;
            }
            else
            {
                bar.gameObject.GetComponent<Image>().color = green;
            }
        }
        
        if (playerMovement.form == "Neutral")
            bar.gameObject.GetComponent<Image>().color = blue;
    }
}
