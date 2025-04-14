using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Karma")]
    public float badKarmaValue;
    public float goodKarmaValue;

    [Header("Shooting")]
    public float shootingDistance;
    private float timeBtwShots;
    public float startTimeBtwShots;
    public float timeBtwShotsMaxAdd;
    private float randTimeBtwShotsAdd;

    private Vector2 lookDirection;
    private float lookAngle;

    [Header("Setup")]
    public GameObject firePoint;
    public GameObject projectile;
    private Transform player;
    private SpriteRenderer spriteRend;
    private Animator enemyAnim;


    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
            player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRend = GetComponent<SpriteRenderer>();
        enemyAnim = GetComponent<Animator>();
        timeBtwShots = startTimeBtwShots;
    }

    void Update()
    {
        if (player != null)
        {
            lookDirection = player.transform.position - transform.position;
            lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            if ((lookAngle > -90 && lookAngle <= 0) || (lookAngle > 0 && lookAngle <= 90))
                spriteRend.flipX = false;
            else if ((lookAngle > 90 && lookAngle <= 180) || (lookAngle > -180 && lookAngle <= -90))
                spriteRend.flipX = true;


            if (timeBtwShots <= 0)
            {
                if (Vector2.Distance(transform.position, player.position) < shootingDistance)
                {
                    enemyAnim.SetBool("Firing", true);

                    GameObject firedMagic = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
                    Magic magic = firedMagic.gameObject.GetComponent<Magic>();

                    magic.randX = Random.Range(-magic.accuracy, magic.accuracy);
                    magic.randY = Random.Range(-magic.accuracy, magic.accuracy);

                    lookDirection = new Vector3(player.transform.position.x + magic.randX, player.transform.position.y + magic.randY) - transform.position;
                    lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                    firePoint.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90f);

                    firedMagic.transform.rotation = firePoint.transform.rotation;
                    firedMagic.GetComponent<Rigidbody2D>().velocity = firePoint.transform.up * magic.speed;
                    magic.friendly = false;
                    randTimeBtwShotsAdd = Random.Range(0, timeBtwShotsMaxAdd);
                    timeBtwShots = startTimeBtwShots + randTimeBtwShotsAdd;
                    Destroy(firedMagic, magic.destroyTime);
                }
            }
            else
            {
                timeBtwShots -= Time.deltaTime;
                if (timeBtwShots < startTimeBtwShots / 2)
                    enemyAnim.SetBool("Firing", false);
            }
        }
        else
            if (GameObject.FindGameObjectWithTag("Player"))
                player = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
