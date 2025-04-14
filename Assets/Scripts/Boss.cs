using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Shooting")]
    private float timeBtwShots;
    public float spinShootTimeBtwShots;
    public float homingTimeBtwShots;
    public float trapShootTimeBtwShots;
    public float randomFireTimeBtwShots;
    public float enemySpawnTimeBtwShots;
    private float randTimeBtwShotsAdd;
    public int shieldRemaining = 0;

    private Vector2 lookDirection;
    private float lookAngle;

    [Header("Phase")]
    public int health;
    public int startingHealth;
    public bool angry = false;
    private string phase;
    private string lastPhase;
    private float timeInPhase;
    public float waitTime;
    public float hurtPhaseTime;
    public float spinShootPhaseTime;
    public float multiSpinShootPhaseTime;
    public float homingShootPhaseTime;
    public float trapSpinPhaseTime;
    public float trapLinePhaseTime;
    public float randomFirePhaseTime;
    public float enemySpawnPhaseTime;
    private int rand;
    public Color shieldedColour;
    public Color standardColour;
    public Color hurtColour;
    private Color colour;

    [Header("Setup")]
    private GameObject firePoint;
    public GameObject firePointR;
    public GameObject firePointL;
    public GameObject firePoint1;
    public GameObject firePoint2;
    public GameObject firePoint3;
    public GameObject firePoint4;
    public GameObject projectile;
    public GameObject homingProjectile;
    public GameObject trapProjectile;
    public GameObject shield4;
    public GameObject shield8;
    public GameObject shield16;
    public GameObject teleporter;
    public Spin spin;
    private Transform player;
    public SpriteRenderer spriteRend;
    private Animator bossAnim;
    public GameObject deathParticles;


    void Start()
    {
        health = startingHealth;
        spin.speed = spin.defaultSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRend = GetComponent<SpriteRenderer>();
        bossAnim = GetComponent<Animator>();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Settings>().PlayMusic(2);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().boss = true;
    }

    void Update()
    {
        if (shieldRemaining == 0)  // Changing colour when being sheilded
        {
            colour = standardColour;
        }
        else
        {
            colour = shieldedColour;
        }


        if (player != null)
        {
            lookDirection = player.transform.position - transform.position;  // Look towards the player
            lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            if ((lookAngle > -90 && lookAngle <= 0) || (lookAngle > 0 && lookAngle <= 90))  // Flip towards the player
            {
                spriteRend.flipX = false;
                firePoint = firePointR;
            }
            else if ((lookAngle > 90 && lookAngle <= 180) || (lookAngle > -180 && lookAngle <= -90))
            {
                spriteRend.flipX = true;
                firePoint = firePointL;
            }


            if (timeInPhase <= 0)
            {
                if (phase == "Wait")
                {
                    spriteRend.color = colour;
                    bossAnim.SetBool("Fire", false);
                    bossAnim.SetBool("ArmsUp", false);
                    rand = Random.Range(1, 8);
                    //rand = 6;

                    if (rand == 1)
                    {
                        phase = "Spin Shoot";
                        spin.speed = spin.spinAttackSpeed;
                        timeInPhase = spinShootPhaseTime;
                        bossAnim.SetBool("ArmsUp", true);
                        if (angry == true)
                        {
                            spin.speed = spin.angrySpinAttackSpeed;
                        }
                    }
                    else if (rand == 2)
                    {
                        phase = "Multi Spin Shoot";
                        spin.speed = spin.multiSpinAttackSpeed;
                        timeInPhase = multiSpinShootPhaseTime;
                        bossAnim.SetBool("ArmsUp", true);
                        if (angry == true)
                        {
                            spin.speed = spin.angryMultiSpinAttackSpeed;
                        }
                    }
                    else if (rand == 3)
                    {
                        phase = "Homing Shoot";
                        spin.speed = spin.homingAttackSpeed;
                        timeInPhase = homingShootPhaseTime;
                        bossAnim.SetBool("Fire", true);
                    }
                    else if (rand == 4)
                    {
                        phase = "Trap Spin";
                        spin.speed = spin.trapAttackSpeed;
                        timeInPhase = trapSpinPhaseTime;
                        bossAnim.SetBool("ArmsUp", true);
                    }
                    else if (rand == 5)
                    {
                        phase = "Trap Line";
                        spin.speed = spin.defaultSpeed;
                        timeInPhase = trapLinePhaseTime;
                        bossAnim.SetBool("Fire", true);
                        if (angry == true)
                        {
                            timeInPhase = trapLinePhaseTime * 2;
                        }
                    }
                    else if (rand == 6)
                    {
                        phase = "Random Shoot";
                        spin.speed = spin.randomFireAttackSpeed;
                        timeInPhase = randomFirePhaseTime;
                        bossAnim.SetBool("ArmsUp", true);
                    }
                    else if (rand == 7)
                    {
                        phase = "Enemy Spawn";
                        spin.speed = spin.enemySpawnSpeed;
                        timeInPhase = enemySpawnPhaseTime;
                        bossAnim.SetBool("ArmsUp", true);
                    }

                    if (phase == lastPhase)
                        timeInPhase = 0;
                }
                else
                {
                    lastPhase = phase;
                    phase = "Wait";
                    timeInPhase = waitTime;
                    bossAnim.SetBool("Fire", false);
                    bossAnim.SetBool("ArmsUp", false);
                    if (spin.speed != spin.shieldSpawnSpeed)
                    {
                        spriteRend.color = colour;
                        spin.speed = spin.defaultSpeed;
                    }
                    else if (angry == true)
                    {
                        spin.speed = spin.angryDefaultSpeed;
                        timeInPhase = waitTime / 1.5f;
                    }
                }
            }



            else if (timeBtwShots <= 0 && timeInPhase > 0 && phase != "Wait")
            {
                if (phase == "Spin Shoot")
                {
                    SpinShoot();
                }
                else if (phase == "Multi Spin Shoot")
                {
                    MultiSpinShoot();
                }
                else if (phase == "Homing Shoot")
                {
                    HomingShoot();
                }
                else if (phase == "Trap Spin")
                {
                    TrapSpin();
                }
                else if (phase == "Trap Line")
                {
                    TrapLine();
                }
                else if (phase == "Random Shoot")
                {
                    RandomShoot();
                }
                else if (phase == "Enemy Spawn")
                {
                    EnemySpawn();
                }
            }
            else
            { 
                timeBtwShots -= Time.deltaTime;
                timeInPhase -= Time.deltaTime;
            }
        }
    }

    void SpinShoot()
    {
        GameObject firedMagic = Instantiate(projectile, firePoint1.transform.position, firePoint1.transform.rotation);
        Magic magic = firedMagic.gameObject.GetComponent<Magic>();

        firedMagic.transform.rotation = firePoint1.transform.rotation;
        firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint1.transform.up * magic.speed;
        magic.friendly = false;
        firedMagic.transform.Find("Fire Particles").gameObject.GetComponent<Sounds>().volume = 3;
        firedMagic.transform.Find("Destroy Particles").gameObject.GetComponent<Sounds>().start = false;
        timeBtwShots = spinShootTimeBtwShots;
        Destroy(firedMagic, magic.destroyTime);

        if (angry == true)
        {
            randTimeBtwShotsAdd = Random.Range(0, 200);
            if (randTimeBtwShotsAdd == 1)
            {
                spin.speed = 0 - spin.angrySpinAttackSpeed;
            }

            firedMagic = Instantiate(projectile, firePoint2.transform.position, firePoint2.transform.rotation);
            magic = firedMagic.gameObject.GetComponent<Magic>();

            firedMagic.transform.rotation = firePoint2.transform.rotation;
            firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint2.transform.up * magic.speed;
            magic.friendly = false;
            firedMagic.transform.Find("Fire Particles").gameObject.GetComponent<Sounds>().start = false;
            firedMagic.transform.Find("Destroy Particles").gameObject.GetComponent<Sounds>().start = false;
            Destroy(firedMagic, magic.destroyTime);
        }
    }

    void MultiSpinShoot()
    {
        GameObject firedMagic = Instantiate(projectile, firePoint1.transform.position, firePoint1.transform.rotation);
        Magic magic = firedMagic.gameObject.GetComponent<Magic>();

        firedMagic.transform.rotation = firePoint1.transform.rotation;
        firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint1.transform.up * magic.speed;
        magic.friendly = false;
        firedMagic.transform.Find("Fire Particles").gameObject.GetComponent<Sounds>().start = false;
        firedMagic.transform.Find("Destroy Particles").gameObject.GetComponent<Sounds>().start = false;
        Destroy(firedMagic, magic.destroyTime);



        firedMagic = Instantiate(projectile, firePoint2.transform.position, firePoint2.transform.rotation);
        magic = firedMagic.gameObject.GetComponent<Magic>();

        firedMagic.transform.rotation = firePoint2.transform.rotation;
        firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint2.transform.up * magic.speed;
        magic.friendly = false;
        firedMagic.transform.Find("Fire Particles").gameObject.GetComponent<Sounds>().start = false;
        firedMagic.transform.Find("Destroy Particles").gameObject.GetComponent<Sounds>().start = false;
        Destroy(firedMagic, magic.destroyTime);



        firedMagic = Instantiate(projectile, firePoint3.transform.position, firePoint3.transform.rotation);
        magic = firedMagic.gameObject.GetComponent<Magic>();

        firedMagic.transform.rotation = firePoint3.transform.rotation;
        firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint3.transform.up * magic.speed;
        magic.friendly = false;
        firedMagic.transform.Find("Fire Particles").gameObject.GetComponent<Sounds>().start = false;
        firedMagic.transform.Find("Destroy Particles").gameObject.GetComponent<Sounds>().start = false;
        Destroy(firedMagic, magic.destroyTime);



        firedMagic = Instantiate(projectile, firePoint4.transform.position, firePoint4.transform.rotation);
        magic = firedMagic.gameObject.GetComponent<Magic>();

        firedMagic.transform.rotation = firePoint4.transform.rotation;
        firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint4.transform.up * magic.speed;
        magic.friendly = false;
        firedMagic.transform.Find("Fire Particles").gameObject.GetComponent<Sounds>().volume = 3;
        firedMagic.transform.Find("Destroy Particles").gameObject.GetComponent<Sounds>().start = false;
        timeBtwShots = spinShootTimeBtwShots;
        Destroy(firedMagic, magic.destroyTime);
        
        if (angry == true && spin.speed > 0)
        {
            randTimeBtwShotsAdd = Random.Range(0, 200);
            if (randTimeBtwShotsAdd == 1)
                spin.speed = 0 - spin.angryMultiSpinAttackSpeed;
        }
    }

    void HomingShoot()
    {
        GameObject firedMagic = Instantiate(homingProjectile, firePoint.transform.position, firePoint.transform.rotation);
        Magic magic = firedMagic.gameObject.GetComponent<Magic>();

        lookDirection = new Vector3(player.transform.position.x + magic.randX, player.transform.position.y + magic.randY) - transform.position;
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        firePoint.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90f);

        firedMagic.transform.rotation = firePoint.transform.rotation;
        firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint.transform.up * magic.speed;
        magic.friendly = false;
        magic.homing = true;
        timeBtwShots = homingTimeBtwShots;
        Destroy(firedMagic, magic.destroyTime);
        if (angry == true)
        {
            timeBtwShots = homingTimeBtwShots / 2;
        }
    }

    void TrapSpin()
    {
        GameObject firedMagic = Instantiate(trapProjectile, firePoint1.transform.position, firePoint1.transform.rotation);
        Magic magic = firedMagic.gameObject.GetComponent<Magic>();

        firedMagic.transform.rotation = firePoint1.transform.rotation;
        firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint1.transform.up * magic.speed;
        magic.friendly = false;
        magic.trap = true;
        firedMagic.transform.Find("Fire Particles").gameObject.GetComponent<Sounds>().volume = 3;
        firedMagic.transform.Find("Destroy Particles").gameObject.GetComponent<Sounds>().volume = 1;
        timeBtwShots = trapShootTimeBtwShots;
        Destroy(firedMagic, magic.destroyTime);
        
        if (angry == true)
        {
            firedMagic = Instantiate(trapProjectile, firePoint2.transform.position, firePoint2.transform.rotation);
            magic = firedMagic.gameObject.GetComponent<Magic>();

            firedMagic.transform.rotation = firePoint2.transform.rotation;
            firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint2.transform.up * magic.speed;
            magic.friendly = false;
            magic.trap = true;
            firedMagic.transform.Find("Fire Particles").gameObject.GetComponent<Sounds>().start = false;
            firedMagic.transform.Find("Destroy Particles").gameObject.GetComponent<Sounds>().volume = 0.3f;
            timeBtwShots = timeBtwShots / 1.5f;
            Destroy(firedMagic, magic.destroyTime);
        }
    }

    void TrapLine()
    {
        GameObject firedMagic = Instantiate(trapProjectile, firePoint.transform.position, firePoint.transform.rotation);
        Magic magic = firedMagic.gameObject.GetComponent<Magic>();

        lookDirection = new Vector3(player.transform.position.x + magic.randX, player.transform.position.y + magic.randY) - transform.position;
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        firePoint.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90f);

        firedMagic.transform.rotation = firePoint.transform.rotation;
        firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint.transform.up * magic.speed;
        magic.friendly = false;
        magic.trap = true;
        firedMagic.transform.Find("Fire Particles").gameObject.GetComponent<Sounds>().volume = 3;
        firedMagic.transform.Find("Destroy Particles").gameObject.GetComponent<Sounds>().volume = 1;
        timeBtwShots = 0.1f;
        Destroy(firedMagic, magic.destroyTime);
    }

    void RandomShoot()
    {
        GameObject firedMagic = Instantiate(projectile, firePoint1.transform.position, firePoint1.transform.rotation);
        Magic magic = firedMagic.gameObject.GetComponent<Magic>();

        firedMagic.transform.rotation = firePoint1.transform.rotation;
        firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint1.transform.up * magic.speed;
        magic.friendly = false;
        firedMagic.transform.Find("Fire Particles").gameObject.GetComponent<Sounds>().volume = 3;
        firedMagic.transform.Find("Destroy Particles").gameObject.GetComponent<Sounds>().start = false;
        randTimeBtwShotsAdd = Random.Range(0, 0.03f);
        timeBtwShots = randomFireTimeBtwShots + randTimeBtwShotsAdd;


        firedMagic = Instantiate(projectile, firePoint2.transform.position, firePoint2.transform.rotation);
        magic = firedMagic.gameObject.GetComponent<Magic>();

        firedMagic.transform.rotation = firePoint2.transform.rotation;
        firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint2.transform.up * magic.speed;
        magic.friendly = false;
        firedMagic.transform.Find("Fire Particles").gameObject.GetComponent<Sounds>().start = false;
        firedMagic.transform.Find("Destroy Particles").gameObject.GetComponent<Sounds>().start = false;
        randTimeBtwShotsAdd = Random.Range(0, 0.03f);
        timeBtwShots = randomFireTimeBtwShots + randTimeBtwShotsAdd;

        if (angry == true)
        {
            firedMagic = Instantiate(projectile, firePoint3.transform.position, firePoint3.transform.rotation);
            magic = firedMagic.gameObject.GetComponent<Magic>();

            firedMagic.transform.rotation = firePoint3.transform.rotation;
            firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint3.transform.up * magic.speed;
            magic.friendly = false;
            firedMagic.transform.Find("Fire Particles").gameObject.GetComponent<Sounds>().start = false;
            firedMagic.transform.Find("Destroy Particles").gameObject.GetComponent<Sounds>().start = false;
            randTimeBtwShotsAdd = Random.Range(0, 0.03f);
            timeBtwShots = randomFireTimeBtwShots + randTimeBtwShotsAdd;


            firedMagic = Instantiate(projectile, firePoint4.transform.position, firePoint4.transform.rotation);
            magic = firedMagic.gameObject.GetComponent<Magic>();

            firedMagic.transform.rotation = firePoint4.transform.rotation;
            firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint4.transform.up * magic.speed;
            magic.friendly = false;
            firedMagic.transform.Find("Fire Particles").gameObject.GetComponent<Sounds>().start = false;
            firedMagic.transform.Find("Destroy Particles").gameObject.GetComponent<Sounds>().start = false;
            randTimeBtwShotsAdd = Random.Range(0, 0.03f);
            timeBtwShots = randomFireTimeBtwShots + randTimeBtwShotsAdd;
            Destroy(firedMagic, magic.destroyTime);
        }
    }

    void EnemySpawn()
    {
        GameObject firedMagic = Instantiate(projectile, firePoint1.transform.position, firePoint1.transform.rotation);
        Magic magic = firedMagic.gameObject.GetComponent<Magic>();

        firedMagic.transform.rotation = firePoint1.transform.rotation;
        firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint1.transform.up * magic.speed;
        magic.friendly = false;
        magic.spawn = true;
        magic.source = this.gameObject;
        timeBtwShots = enemySpawnTimeBtwShots;
        Destroy(firedMagic, magic.destroyTime);
        if (angry == true)
        {
            firedMagic = Instantiate(projectile, firePoint2.transform.position, firePoint2.transform.rotation);
            magic = firedMagic.gameObject.GetComponent<Magic>();

            firedMagic.transform.rotation = firePoint2.transform.rotation;
            firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint2.transform.up * magic.speed;
            magic.friendly = false;
            magic.spawn = true;
            magic.source = this.gameObject;
            firedMagic.transform.Find("Fire Particles").gameObject.GetComponent<Sounds>().start = false;
            firedMagic.transform.Find("Destroy Particles").gameObject.GetComponent<Sounds>().start = false;
        }
    }

    public void Hurt()
    {
        phase = "Hurt";
        timeInPhase = hurtPhaseTime;
        spin.speed = spin.shieldSpawnSpeed;
        health--;
        if (health > 0)
            gameObject.GetComponent<Sounds>().Play();

        if (health == 4)
        {
            GameObject shield = Instantiate(shield4, transform.position, spin.transform.rotation, transform.Find("Spin").Find("Shield"));
            shieldRemaining = 4;
        }

        else if (health == 3)
        {
            GameObject shield = Instantiate(shield8, transform.position, spin.transform.rotation, transform.Find("Spin").Find("Shield"));
            shieldRemaining = 8;
        }

        else if (health == 1 || health == 2)
        {
            angry = true;
            GameObject shield = Instantiate(shield16, transform.position, spin.transform.rotation, transform.Find("Spin").Find("Shield"));
            shieldRemaining = 16;
        }
        else if (health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Instantiate(teleporter, transform.position, transform.rotation);
        deathParticles.SetActive(true);
        deathParticles.transform.parent = transform.parent.parent;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Settings>().PlayMusic(0);
        foreach (Transform child in transform.parent)
        {
            if (child.name != "Boss")
                Destroy(child.gameObject);
        }
        Destroy(gameObject);
    }
}
