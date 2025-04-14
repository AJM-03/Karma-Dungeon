using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1=Bottom
    // 2=Top
    // 3=Left
    // 4=Right

    private Templates templates;
    private int rand;
    private bool spawned = false;

    public float waitTime = 4f;

    void Start()
    {
        Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<Templates>();

        Invoke("Spawn", 0.1f);  // Will stop all rooms from spawning at once
    }

    void Spawn()
    {
        if(spawned == false)
        {
            if (openingDirection == 1)
            {
                // Spawn with bottom door
                rand = Random.Range(0, templates.bottomRooms.Length);
                GameObject room = Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation, GameObject.Find("Rooms").transform);
                templates.rooms.Add(room);
            }

            else if (openingDirection == 2)
            {
                // Spawn with Top door
                rand = Random.Range(0, templates.topRooms.Length);
                GameObject room = Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation, GameObject.Find("Rooms").transform);
                templates.rooms.Add(room);
            }

            else if (openingDirection == 3)
            {
                // Spawn with Left door
                rand = Random.Range(0, templates.leftRooms.Length);
                GameObject room = Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation, GameObject.Find("Rooms").transform);
                templates.rooms.Add(room);
            }

            else if (openingDirection == 4)
            {
                // Spawn with Right door
                rand = Random.Range(0, templates.rightRooms.Length);
                GameObject room = Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation, GameObject.Find("Rooms").transform);
                templates.rooms.Add(room);
            }
            spawned = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Room Spawn") && collision.GetComponent<RoomSpawner>() != null)
        {
            if(collision.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity, GameObject.Find("Rooms").transform);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
