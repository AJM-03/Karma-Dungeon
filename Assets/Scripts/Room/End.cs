using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    private GameObject room;
    public GameObject skull;
    private bool spawned;

    void Update()
    {
        if (room != null && spawned == false)
        {
            transform.parent = room.transform.parent;
            //transform.parent = room.transform.parent.Find("Disable On Enter");
            GameObject.FindGameObjectWithTag("Rooms").GetComponent<Templates>();
            GameObject boss = Instantiate(GameObject.FindGameObjectWithTag("Rooms").GetComponent<Templates>().boss, transform.position, GameObject.FindGameObjectWithTag("Rooms").GetComponent<Templates>().boss.transform.rotation, room.transform.parent.Find("Enable On Enter").Find("Enemies"));
            
            //GameObject bossDoor = Instantiate(GameObject.FindGameObjectWithTag("Rooms").GetComponent<Templates>().boss, transform.position, GameObject.FindGameObjectWithTag("Rooms").GetComponent<Templates>().boss.transform.rotation, room.transform.parent.Find("Enable On Enter").Find("Enemies"));

            foreach (Transform child in room.transform.parent.Find("Enable On Enter").Find("Enemy Spawns"))
            {
                Destroy(child.gameObject);
            }
            room.transform.parent.Find("Center").gameObject.GetComponent<EnemiesRemaining>().enabled = false;
            spawned = true;
        }

        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Settings>().bossOnMapSetting == true)
        {
            skull.SetActive(true);
        }
        else
        {
            skull.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room Center"))
            room = collision.gameObject;

        else if (collision.CompareTag("Player"))
            transform.parent = room.transform.parent.Find("Disable On Enter");
    }
}
