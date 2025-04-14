using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public string type;

    private Templates templates;
    private int rand;
    private bool spawned = false;
    public bool attack = false;
    private int roomNum;
    private GameObject newShield;

    public float waitTime = 4f;
    public float shieldSpawnWait;

    void Start()
    {
        Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<Templates>();

        roomNum = templates.rooms.Count;
        if (attack == false)
            if (type == "Shield" || type == "None")
                Invoke("Spawn", shieldSpawnWait);  // Will stop all enemies from spawning at once
            else
                Invoke("Spawn", 0.1f);  // Will stop all enemies from spawning at once
        else
            Invoke("Spawn", 0);
    }

    void Spawn()
    {
        if (spawned == false)
        {
            if (type == "Enemy")
            {
                if (attack == false)
                {
                    if (roomNum < templates.maxRooms / 2)
                    {
                        rand = Random.Range(0, templates.standardEnemies.Length);
                        GameObject enemy = Instantiate(templates.standardEnemies[rand], transform.position, templates.standardEnemies[rand].transform.rotation, transform.parent.parent.Find("Enemies"));
                        Debug.Log("Easy");
                    }
                    else
                    {
                        rand = Random.Range(0, templates.hardEnemies.Length);
                        GameObject enemy = Instantiate(templates.hardEnemies[rand], transform.position, templates.hardEnemies[rand].transform.rotation, transform.parent.parent.Find("Enemies"));
                    }
                }
                else
                {
                    if (GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().angry == false)
                    {
                        rand = Random.Range(0, templates.bossEnemies.Length);
                        GameObject enemy = Instantiate(templates.bossEnemies[rand], transform.position, templates.bossEnemies[rand].transform.rotation, transform.parent);
                    }
                    else
                    {
                        rand = Random.Range(0, templates.hardBossEnemies.Length);
                        GameObject enemy = Instantiate(templates.hardBossEnemies[rand], transform.position, templates.hardBossEnemies[rand].transform.rotation, transform.parent);
                    }
                }
            }


            else if (type == "Object")
            {
                rand = Random.Range(0, templates.objects.Length);
                Instantiate(templates.objects[rand], transform.position, templates.objects[rand].transform.rotation, transform.parent.parent.Find("Objects"));
            }

            else if (type == "Shield")
            {
                if (GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().health == 4)
                    newShield = Instantiate(templates.shield4Part, transform.position, transform.rotation, transform.parent.parent);
                else if (GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().health == 3)
                    newShield = Instantiate(templates.shield8Part, transform.position, transform.rotation, transform.parent.parent);
                else if (GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().health == 2 || GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().health == 1)
                    newShield = Instantiate(templates.shield16Part, transform.position, transform.rotation, transform.parent.parent);
            }

            else if (type == "None")
                GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().spriteRend.color = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().shieldedColour;

            spawned = true;
            Destroy(gameObject);
        }
    }
}
