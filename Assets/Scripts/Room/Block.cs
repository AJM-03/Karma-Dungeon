using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private float waitTime = 2f;
    private bool activatorCollision = false;
    private GameObject activator;
    private GameObject roomCenter;
    public int direction;
    public bool closedRoom;

    private int rand;
    public bool spawned = false;
    private Templates templates;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<Templates>();
    }

    private void Update()
    {
        if (waitTime <= 0 && spawned == false && closedRoom == false)
        {

            if (roomCenter != null)
            {
                if (activatorCollision == false)
                {
                    Destroy(gameObject);
                }

                else if (direction == 1)
                {
                    // Spawn with bottom door
                    rand = Random.Range(0, templates.bottomBlocks.Length);
                    GameObject block = Instantiate(templates.bottomBlocks[rand], transform.position, templates.bottomBlocks[rand].transform.rotation, roomCenter.transform.parent.Find("Enable On Enter").Find("Grid").Find("Blocks"));
                    block.transform.Find("White Block B").gameObject.GetComponent<MoveBlock>().spawned = true;
                }

                else if (direction == 2)
                {
                    // Spawn with Top door
                    rand = Random.Range(0, templates.topBlocks.Length);
                    GameObject block = Instantiate(templates.topBlocks[rand], transform.position, templates.topBlocks[rand].transform.rotation, roomCenter.transform.parent.Find("Enable On Enter").Find("Grid").Find("Blocks"));
                    block.transform.Find("White Block T").gameObject.GetComponent<MoveBlock>().spawned = true;
                }

                else if (direction == 3)
                {
                    // Spawn with Left door
                    rand = Random.Range(0, templates.leftBlocks.Length);
                    GameObject block = Instantiate(templates.leftBlocks[rand], transform.position, templates.leftBlocks[rand].transform.rotation, roomCenter.transform.parent.Find("Enable On Enter").Find("Grid").Find("Blocks"));
                    block.transform.Find("White Block L").gameObject.GetComponent<MoveBlock>().spawned = true;
                }

                else if (direction == 4)
                {
                    // Spawn with Right door
                    rand = Random.Range(0, templates.rightBlocks.Length);
                    GameObject block = Instantiate(templates.rightBlocks[rand], transform.position, templates.rightBlocks[rand].transform.rotation, roomCenter.transform.parent.Find("Enable On Enter").Find("Grid").Find("Blocks"));
                    block.transform.Find("White Block R").gameObject.GetComponent<MoveBlock>().spawned = true;
                }
                spawned = true;
            }
            Destroy(activator);
            Destroy(gameObject);
        }

        else if (waitTime <= 0 && closedRoom == true)
        {
            if (activatorCollision == false)
                Destroy(gameObject);

            else if (activatorCollision == true && roomCenter != null)
            {
                transform.parent = roomCenter.transform.parent.Find("Enable On Enter").Find("Grid").Find("Blocks");
                if (transform.Find("White Block"))
                    transform.Find("White Block").gameObject.GetComponent<MoveBlock>().spawned = true;
            }
        }

        else
        {
            waitTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Block Activator"))
        {
            activatorCollision = true;
            activator = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("Room Center"))
        {
            roomCenter = collision.gameObject;
        }
    }
}