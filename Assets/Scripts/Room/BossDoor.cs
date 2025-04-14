using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public GameObject exitBlock;
    private Templates templates;
    private GameObject oldRoom;
    private bool spawned = false;
    private bool boss = false;
    private bool activatorCollision = false;
    private GameObject roomCenter;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<Templates>();
        oldRoom = this.transform.parent.parent.gameObject;
    }


    void Update()
    {
        if (templates.spawnedEnd == true && spawned == false)
        {
            transform.parent = templates.gameObject.transform.parent;

            if (templates.bossRoom == oldRoom)
            {
                boss = true;
                spawned = true;
                if (exitBlock != null)
                    exitBlock.transform.parent = oldRoom.transform.Find("Enable On Enter").Find("Grid").Find("Blocks");
            }

            else
                Destroy(gameObject);
        }

        if (boss == true && activatorCollision == true && roomCenter != null)
        {
            transform.parent = roomCenter.transform.parent.Find("Enable On Enter").Find("Grid").Find("Blocks");
            boss = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Block Activator"))
        {
            activatorCollision = true;
        }

        if (collision.gameObject.CompareTag("Room Center") && collision.gameObject != oldRoom.transform.Find("Center").gameObject)
        {
            roomCenter = collision.gameObject;
        }
    }
}
