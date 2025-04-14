using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    private Karma karma;
    public float speed;
    public float destroyTime;
    public float accuracy;
    public bool friendly = true;
    public bool homing = false;
    public bool trap = false;
    public bool spawn = false;
    public bool shield = false;
    public bool indestructible = false;
    public bool canBreak = true;
    public bool canBreakStrong = false;
    public float rotateSpeed;
    public GameObject source;


    private Transform player;
    private Rigidbody2D rb2d;
    public float randX;
    public float randY;
    private Vector2 lookDirection;
    private float lookAngle;
    private float timeTillFreeze;
    public GameObject enemySpawner;
    public GameObject destroyParticles;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (friendly == true)
        {
            if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Room Spawn") && !collision.gameObject.CompareTag("Room Center") && !collision.gameObject.CompareTag("Pit"))
            {
                karma = GameObject.FindGameObjectWithTag("Player").GetComponent<Karma>();
                
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                    karma.KarmaAdd(enemy.badKarmaValue);
                    destroyParticles.SetActive(true);
                    destroyParticles.transform.parent = transform.parent;
                    collision.transform.Find("Death Particles").gameObject.SetActive(true);
                    collision.transform.Find("Death Particles").parent = transform.parent;
                    Destroy(collision.gameObject);
                }

                else if (collision.gameObject.CompareTag("Breakable") && canBreak == true)
                {
                    collision.transform.Find("Particles").gameObject.SetActive(true);
                    Destroy(collision.transform.Find("Particles").gameObject, 2);
                    collision.transform.Find("Particles").parent = collision.transform.parent;
                    Destroy(collision.gameObject);
                    if (indestructible == false)
                    {
                        destroyParticles.SetActive(true);
                        destroyParticles.transform.parent = transform.parent;
                        Destroy(gameObject);
                    }
                }

                else if (collision.gameObject.CompareTag("Breakable (Strong)") && canBreakStrong == true)
                {
                    collision.transform.Find("Particles").gameObject.SetActive(true);
                    Destroy(collision.transform.Find("Particles").gameObject, 2);
                    collision.transform.Find("Particles").parent = collision.transform.parent;
                    Destroy(collision.gameObject);
                }

                else if (collision.gameObject.CompareTag("Magic") && collision.GetComponent<Magic>().shield == true)
                {
                    GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().shieldRemaining--;
                    if (GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().shieldRemaining == 0)
                        GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().spriteRend.color = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().standardColour;
                    Destroy(collision.gameObject);
                    if (destroyParticles != null)
                    {
                        destroyParticles.SetActive(true);
                        destroyParticles.transform.parent = transform.parent;
                    }
                    Destroy(gameObject);
                }

                else if (collision.gameObject.CompareTag("Boss") && GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().shieldRemaining == 0)
                {
                    GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().spriteRend.color = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().hurtColour;
                    GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().Hurt();
                }

                if (collision.gameObject.CompareTag("Magic") && collision.GetComponent<Magic>().friendly == true) { }

                else if (indestructible == false)
                {
                    destroyParticles.SetActive(true);
                    destroyParticles.transform.parent = transform.parent;
                    Destroy(gameObject);
                }
            }
        }




        else if (friendly == false)
        {
            if (!collision.gameObject.CompareTag("Enemy") && !collision.gameObject.CompareTag("Room Center") && !collision.gameObject.CompareTag("Room Spawn") && !collision.gameObject.CompareTag("Boss") && !collision.gameObject.CompareTag("Pit"))
            {
                if (collision.gameObject.CompareTag("Player") && player.GetComponent<PlayerMovement>().dashing == false && destroyParticles != null)
                {
                    player.GetComponent<PlayerMovement>().Kill();
                    destroyParticles.SetActive(true);
                    destroyParticles.transform.parent = transform.parent;
                    Destroy(gameObject);
                }

                else if (collision.gameObject.CompareTag("Player") && player.GetComponent<PlayerMovement>().dashing == true) { }

                else if (collision.gameObject.CompareTag("Breakable") && canBreak == true)
                {
                    collision.transform.Find("Particles").gameObject.SetActive(true);
                    Destroy(collision.transform.Find("Particles").gameObject, 2);
                    collision.transform.Find("Particles").parent = collision.transform.parent;
                    Destroy(collision.gameObject);
                    if (indestructible == false)
                    {
                        if (destroyParticles != null)
                        {
                            destroyParticles.SetActive(true);
                            destroyParticles.transform.parent = transform.parent;
                        }
                        if (shield == true)
                            GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().shieldRemaining--;

                        Destroy(gameObject);
                    }
                }

                else if (collision.gameObject.CompareTag("Breakable (Strong)") && canBreakStrong == true)
                {
                    collision.transform.Find("Particles").gameObject.SetActive(true);
                    Destroy(collision.transform.Find("Particles").gameObject, 2);
                    collision.transform.Find("Particles").parent = collision.transform.parent;
                    Destroy(collision.gameObject);
                }

                else if (collision.gameObject.CompareTag("Magic") && collision.GetComponent<Magic>().shield == true) { }
                
                else if (shield == true) { }

                else if (collision.gameObject.CompareTag("Magic") && collision.GetComponent<Magic>().homing == true && homing == true) { }

                else if (collision.gameObject.CompareTag("Magic") && collision.GetComponent<Magic>().trap == true && trap == true) { }

                else
                {
                    destroyParticles.SetActive(true);
                    destroyParticles.transform.parent = transform.parent;
                    Destroy(gameObject);
                }
            }
        }
    }



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb2d = GetComponent<Rigidbody2D>();

        if (trap == true)
            timeTillFreeze = Random.Range(0.25f, 2f);
        if (spawn == true)
            timeTillFreeze = Random.Range(0.25f, 1.4f);
    }



    private void Update()
    {
        if (homing == true && player != null)
        {
            lookDirection = player.transform.position - transform.position;
            lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

            if (lookAngle - transform.rotation.z < 0)
            {
                lookDirection.Normalize();

                float rotateAmount = Vector3.Cross(lookDirection, transform.up).z;

                rb2d.angularVelocity = -rotateAmount * rotateSpeed;

                rb2d.velocity = transform.up;

                rb2d.velocity = transform.up * speed;
            }
        }

        if (trap == true)
        {
            if (timeTillFreeze <= 0)
            {
                rb2d.velocity = Vector3.zero;
                rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
                timeTillFreeze -= Time.deltaTime;
            }
        }

        if (spawn == true)
        {
            if (timeTillFreeze <= 0)
            {
                Instantiate(enemySpawner, transform.position, enemySpawner.transform.rotation, source.transform.parent);
                Destroy(gameObject);
            }
            else
            {
                timeTillFreeze -= Time.deltaTime;
            }
        }
    }
}