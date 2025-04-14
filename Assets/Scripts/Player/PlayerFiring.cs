using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFiring : MonoBehaviour
{
    public GameObject player;
    private PlayerMovement movement;
    public GameObject spin;
    public Transform playerPosition;
    private Vector2 lookDirection;
    public GameObject firePoint;
    private float lookAngle;
    private float timer;
    private float timer2;
    private float abilityTimer;
    public GameObject canvas;

    [Header("Neutral Magic")]
    public GameObject neutralMagic;
    public float neutralFireRate;

    [Header("Good Magic")]
    public GameObject goodMagic;
    public float goodFireRate;

    [Header("Bad Magic")]
    public GameObject badMagic;
    public float badFireRate;

    [Header("Bad Ability")]
    public GameObject badAbility;
    public float badAbilityFireRate;

    [Header("Neutral Ability")]
    public GameObject neutralAbility;
    public float neutralAbilityFireRate;
    private GameObject firedNeutralAbility;

    private void Start()
    {
        movement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90f);

        if (movement.form != "Soul" && canvas.GetComponent<MainMenu>().paused == false)
        {
            if (timer <= 0)
            {
                if (Input.GetButtonDown("Fire"))
                {
                    movement.playerAnim.SetBool("Fire", true);
                    movement.playerAnim.SetBool("ArmsUp", false);

                    if (movement.form == "Neutral")
                    {
                        FireNeutralMagic();
                        timer = neutralFireRate;
                    }

                    else if (movement.form == "Good")
                    {
                        FireGoodMagic();
                        timer = goodFireRate;
                    }

                    else if (movement.form == "Bad")
                    {
                        FireBadMagic();
                        timer = badFireRate;
                    }
                }

                else if (timer <= -2)
                    movement.playerAnim.SetBool("Fire", false);
            }

            timer -= Time.deltaTime;


            if (abilityTimer <= 0)
            {
                if (Input.GetButtonDown("Ability"))
                {
                    if (movement.form == "Bad")
                    {
                        FireBadAbility();
                        abilityTimer = badAbilityFireRate;
                    }

                    else if (movement.form == "Neutral")
                    {
                        NeutralAbility();
                    }
                }
            }
            else
            {
                abilityTimer -= Time.deltaTime;
            }

            /*if ((lookAngle > -45 && lookAngle <= 0) || (lookAngle > 0 && lookAngle <= 45))
                Debug.Log("Right");

            else if (lookAngle > 45 && lookAngle <= 135)
                Debug.Log("Up");

            else if ((lookAngle > 135 && lookAngle <= 180) || (lookAngle > -180 && lookAngle <= -135))
                Debug.Log("Left");

           else if (lookAngle > -135 && lookAngle <= -45)
                Debug.Log("Down");*/
        }

        if (movement.dashing == false)
        {
            if ((lookAngle > -90 && lookAngle <= 0) || (lookAngle > 0 && lookAngle <= 90))
                movement.currentSprite.flipX = false;
            else if ((lookAngle > 90 && lookAngle <= 180) || (lookAngle > -180 && lookAngle <= -90))
                movement.currentSprite.flipX = true;
        }
    }

    private void FireNeutralMagic()
    {
        GameObject firedMagic = Instantiate(neutralMagic, firePoint.transform.position, this.transform.rotation);
        Magic magic = firedMagic.gameObject.GetComponent<Magic>();
        firedMagic.GetComponent<Rigidbody2D>().velocity = this.transform.up * magic.speed;
        Destroy(firedMagic, magic.destroyTime);
    }

    private void FireGoodMagic()
    {
        GameObject firedMagic = Instantiate(goodMagic, firePoint.transform.position, this.transform.rotation);
        Magic magic = firedMagic.gameObject.GetComponent<Magic>();
        firedMagic.GetComponent<Rigidbody2D>().velocity = this.transform.up * magic.speed;
        Destroy(firedMagic, magic.destroyTime);
    }

    private void FireBadMagic()
    {
        GameObject firedMagic = Instantiate(badMagic, firePoint.transform.position, this.transform.rotation);
        Magic magic = firedMagic.gameObject.GetComponent<Magic>();
        firedMagic.GetComponent<Rigidbody2D>().velocity = this.transform.up * magic.speed;
        Destroy(firedMagic, magic.destroyTime);
    }

    private void FireBadAbility()
    {
        movement.playerAnim.SetBool("ArmsUp", true);
        GameObject firedMagic = Instantiate(badAbility, player.transform.position + new Vector3(0, -0.6f, 0), player.transform.rotation);
        Magic magic = firedMagic.gameObject.GetComponent<Magic>();
        firedMagic.GetComponent<Rigidbody2D>().velocity = this.transform.up * magic.speed;
        Destroy(firedMagic, magic.destroyTime);
    }

    void NeutralAbility()
    {

    }
}

