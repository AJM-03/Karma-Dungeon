using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float neutralSpeed;
    public float goodSpeed;
    public float badSpeed;
    public float soulSpeed;
    private float formSpeed;
    private float speed;
    private Vector3 movementChange;

    [Header("Dash")]
    public float dashSpeed;
    public float dashTime;
    public bool dashing = false;
    public float goodAbilityFireRate;
    public GameObject dashEffect;

    [Header("Random")]
    public float invinciblityTime;
    public string form;
    public GameObject soul;
    public GameObject good;
    public GameObject bad;
    public Karma karma;
    private bool invincible;
    private float invinciblityTimeLeft;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRend;
    public SpriteRenderer currentSprite;
    public GameObject rebirthBar;
    public Animator playerAnim;
    public GameObject lastTransfer;
    private int fallTime = 0;
    private float abilityTimer;
    public bool boss = false;

    [Header("Particles")]
    public GameObject soulParticles;
    public GameObject deathParticles;
    public GameObject dustParticles;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        currentSprite = GetComponent<SpriteRenderer>();
        playerAnim = GetComponent<Animator>();
        karma = GetComponent<Karma>();
        formSpeed = neutralSpeed;
        Camera.main.GetComponent<CameraMovement>().target = gameObject.transform;
    }


    void Update()
    {
        movementChange = Vector2.zero;
        movementChange.x = Input.GetAxisRaw("Horizontal");  // GetAxisRaw goes straight to 1 instead of building up to it
        movementChange.y = Input.GetAxisRaw("Vertical");

        if (abilityTimer > 0)
            abilityTimer -= Time.deltaTime;


        if (Input.GetButtonDown("Ability"))
        {
            if (form == "Good")
            {
                if (abilityTimer <= 0)
                {
                    abilityTimer = goodAbilityFireRate;
                    StartCoroutine(Dash());
                }
            }


            else if (form == "Neutral" && boss == false)
            {
                if (rebirthBar.activeSelf == false)
                {
                    rebirthBar.GetComponent<RadialSliderChange>().currentAmount = 0f;
                    rebirthBar.SetActive(true);
                }
            }

            else if (form == "Soul")
            {
                if (rebirthBar.activeSelf == false)
                {
                    rebirthBar.GetComponent<RadialSliderChange>().currentAmount = 0f;
                    rebirthBar.SetActive(true);
                }
            }
        }

        if (invincible == true)
        {
            if (invinciblityTimeLeft <= 0)
            {
                invincible = false;
            }
            else
            {
                invinciblityTimeLeft -= Time.deltaTime;
            }
        }

        if (movementChange != Vector3.zero)
        {
            if (rebirthBar.activeSelf == true)
                rebirthBar.SetActive(false);

            if (dashing == false)
            {
                speed = formSpeed;
                if ((movementChange.x == 1 || movementChange.x == -1) && (movementChange.y == 1 || movementChange.y == -1))  // When travelling diagonally
                    speed = speed / 1.3f;  // Slows you down
            }

            dustParticles.SetActive(true);
            MoveCharacter();
        }
        else
        {
            dustParticles.SetActive(false);
            playerAnim.SetBool("Moving", false);
        }
    }


    void MoveCharacter()
    {
        playerAnim.SetBool("ArmsUp", false);
        playerAnim.SetBool("Moving", true);
        rb2d.position = transform.position + movementChange * speed * Time.fixedDeltaTime;
    }


    private IEnumerator Dash()
    {
        speed = dashSpeed;
        dashing = true;
        if ((movementChange.x == 1 || movementChange.x == -1) && (movementChange.y == 1 || movementChange.y == -1))
            speed = speed / 1.5f;

        GameObject DashEffect = Instantiate(dashEffect, transform.position, dashEffect.transform.rotation);
        DashEffect.GetComponent<SpriteRenderer>().sprite = currentSprite.sprite;
        DashEffect.GetComponent<SpriteRenderer>().flipX = currentSprite.flipX;
        Destroy(DashEffect, 0.1f);

        yield return new WaitForSeconds(dashTime);
        playerAnim.SetBool("ArmsUp", false);
        transform.localScale = new Vector3(0.95f, 1f, 1);
        speed = goodSpeed;
        dashing = false;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss") || collision.gameObject.CompareTag("Enemy Projectile")) && dashing == false)
        {
            Kill();
        }
        else if (collision.gameObject.CompareTag("Pit") && dashing == false && form != "Soul")
        {
            if (fallTime < 25)
            {
                fallTime = fallTime + 1;
                transform.localScale = transform.localScale - new Vector3 (0.01f, 0.01f, 0);
            }
            else
                Fall();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pit") && dashing == false && form != "Soul")
        {
            fallTime = 0;
            playerAnim.SetBool("ArmsUp", false);
            transform.localScale = new Vector3(0.95f, 1f, 1);
        }
    }

    public void Kill()
    {
        if (form != "Soul" && invincible == false)
        {
            Speak(3);
            invincible = true;
            invinciblityTimeLeft = invinciblityTime;
            spriteRend.enabled = false;
            soul.GetComponent<SpriteRenderer>().enabled = true;
            currentSprite = soul.GetComponent<SpriteRenderer>();
            bad.GetComponent<SpriteRenderer>().enabled = false;
            good.GetComponent<SpriteRenderer>().enabled = false;
            spriteRend.enabled = false;
            form = "Soul";
            formSpeed = soulSpeed;
            soulParticles.SetActive(true);
        }

        else if (form == "Soul" && invincible == false)
        {
            invincible = true;
            invinciblityTimeLeft = invinciblityTime;
            deathParticles.SetActive(true);
            deathParticles.transform.parent = transform.parent;
            Destroy(gameObject);
        }
    }

    public void Rebirth()
    {
        if (karma.karma > 55)
        {
            form = "Good";
            formSpeed = goodSpeed;
            good.GetComponent<SpriteRenderer>().enabled = true;
            bad.GetComponent<SpriteRenderer>().enabled = false;
            currentSprite = good.GetComponent<SpriteRenderer>();
            playerAnim = good.GetComponent<Animator>();
        }
        else if (karma.karma < 45)
        {
            form = "Bad";
            formSpeed = badSpeed;
            bad.GetComponent<SpriteRenderer>().enabled = true;
            good.GetComponent<SpriteRenderer>().enabled = false;
            currentSprite = bad.GetComponent<SpriteRenderer>();
            playerAnim = bad.GetComponent<Animator>();
        }
        else
        {
            form = "Neutral";
            formSpeed = neutralSpeed;
            spriteRend.enabled = true;
            currentSprite = GetComponent<SpriteRenderer>();
            good.GetComponent<SpriteRenderer>().enabled = false;
            bad.GetComponent<SpriteRenderer>().enabled = false;
            playerAnim = GetComponent<Animator>();
        }

        Speak(1);
        playerAnim.SetBool("ArmsUp", true);
        soul.GetComponent<SpriteRenderer>().enabled = false;
        soulParticles.SetActive(false);
        karma.KarmaReset();
    }

    public void Speak(int context)
    {
        if(form == "Neutral")
        {
            if(context == 1)
            {
                gameObject.transform.Find("Sounds").Find("Neutral Intro").GetComponent<Sounds>().Play();
            }
            else if (context == 2)
            {
                gameObject.transform.Find("Sounds").Find("Neutral Kill").GetComponent<Sounds>().Play();
            }
            else if (context == 3)
            {
                gameObject.transform.Find("Sounds").Find("Neutral Death").GetComponent<Sounds>().Play();
            }
        }
        else if (form == "Good")
        {
            if (context == 1)
            {
                gameObject.transform.Find("Sounds").Find("Good Intro").GetComponent<Sounds>().Play();
            }
            else if (context == 2)
            {
                //gameObject.transform.Find("Sounds").Find("Good Kill").GetComponent<Sounds>().Play();
            }
            else if (context == 3)
            {
                gameObject.transform.Find("Sounds").Find("Good Death").GetComponent<Sounds>().Play();
            }
        }
        else if (form == "Bad")
        {
            if (context == 1)
            {
                gameObject.transform.Find("Sounds").Find("Bad Intro").GetComponent<Sounds>().Play();
            }
            else if (context == 2)
            {
                gameObject.transform.Find("Sounds").Find("Bad Kill").GetComponent<Sounds>().Play();
            }
            else if (context == 3)
            {
                gameObject.transform.Find("Sounds").Find("Bad Death").GetComponent<Sounds>().Play();
            }
        }
    }

    public void Return()
    {
        Camera.main.GetComponent<CameraMovement>().currentRoom = Camera.main.GetComponent<CameraMovement>().startingRoom.transform.Find("Center").gameObject;
        Camera.main.GetComponent<CameraMovement>().startingRoom.transform.Find("Enable On Enter").gameObject.SetActive(true);
        Camera.main.GetComponent<CameraMovement>().startingRoom.transform.Find("Disable On Enter").gameObject.SetActive(false);
        transform.position = Camera.main.GetComponent<CameraMovement>().currentRoom.transform.position;
        Camera.main.GetComponent<CameraMovement>().maxPosition = new Vector3(4.5f, 5.5f);
        Camera.main.GetComponent<CameraMovement>().minPosition = new Vector3(-4.5f, -5.5f);
    }

    void Fall()
    {
        Speak(3);
        transform.position = lastTransfer.transform.position;
        playerAnim.SetBool("ArmsUp", false);
        transform.localScale = new Vector3(0.95f, 1f, 1);
    }
}