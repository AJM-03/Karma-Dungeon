using System.Collections.Generic;
using UnityEngine;

public class Templates : MonoBehaviour
{
    public float maxRooms;

    public float waitTime;
    public bool spawnedEnd = false;
    public GameObject bossRoom;
    public GameObject end;

    public GameObject closedRoom;
    public GameObject boss;


    [Header("Standard")]
    public GameObject[] standardBottomRooms;
    public GameObject[] standardTopRooms;
    public GameObject[] standardLeftRooms;
    public GameObject[] standardRightRooms;

    [Header("Blocks")]
    public GameObject[] bottomBlocks;
    public GameObject[] topBlocks;
    public GameObject[] leftBlocks;
    public GameObject[] rightBlocks;

    [Header("End")]
    public GameObject[] endBottomRooms;
    public GameObject[] endTopRooms;
    public GameObject[] endLeftRooms;
    public GameObject[] endRightRooms;

    [Header("Enemies")]
    public GameObject[] standardEnemies;
    public GameObject[] hardEnemies;
    public GameObject[] bossEnemies;
    public GameObject[] hardBossEnemies;

    [Header("Objects")]
    public GameObject[] objects;

    [Header("Boss Door")]
    public GameObject bottomBossDoor;
    public GameObject topBossDoor;
    public GameObject leftBossDoor;
    public GameObject rightBossDoor;

    [Header("Boss Shield")]
    public GameObject shield4Part;
    public GameObject shield8Part;
    public GameObject shield16Part;

    [Header("Don't Change")]
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    [Header("Room List")]
    public List<GameObject> rooms;


    private void Start()
    {
        bottomRooms = standardBottomRooms;
        topRooms = standardTopRooms;
        leftRooms = standardLeftRooms;
        rightRooms = standardRightRooms;

        maxRooms = PlayerPrefs.GetFloat("DungeonSize");
    }


    private void Update()
    {

        if (rooms.Count >= maxRooms)
        {
            bottomRooms = endBottomRooms;
            topRooms = endTopRooms;
            leftRooms = endLeftRooms;
            rightRooms = endRightRooms;
        }

        if (waitTime <= 0 && spawnedEnd == false)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if(i == rooms.Count - 1)
                {
                    Instantiate(end, rooms[i].transform.position, Quaternion.identity);
                    bossRoom = rooms[i];
                    spawnedEnd = true;
                }
            }
        }
        else
        {
            if(spawnedEnd == false)
                waitTime -= Time.deltaTime;
        }
    }
}
