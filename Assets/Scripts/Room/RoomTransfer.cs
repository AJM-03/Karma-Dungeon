using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransfer : MonoBehaviour
{
    public Vector2 halfRoomSize;
    public GameObject roomCenterObj;
    public Vector3 playerChange;
    private Vector2 roomCenter;
    private CameraMovement cam;
    public GameObject enableOnEnter;
    public GameObject disableOnEnter;
    public bool mainTransfer;

    private float waitTime;
    private float setWaitTime = 0.8f;

    void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();
        roomCenter = roomCenterObj.transform.position;
        waitTime = setWaitTime;

        disableOnEnter.SetActive(false);
        //enableOnEnter.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            waitTime = setWaitTime;

            enableOnEnter.SetActive(true);
            disableOnEnter.SetActive(false);

            cam.currentRoom = roomCenterObj;

            cam.minPosition = roomCenter;
            cam.minPosition.x = cam.minPosition.x - halfRoomSize.x;
            cam.minPosition.y = cam.minPosition.y - halfRoomSize.y;

            cam.maxPosition = roomCenter;
            cam.maxPosition.x = cam.maxPosition.x + halfRoomSize.x;
            cam.maxPosition.y = cam.maxPosition.y + halfRoomSize.y;

            if (playerChange == new Vector3(2.4f, 0, 0))
                collision.transform.position = roomCenterObj.transform.position + new Vector3(-13f,0);
            else if (playerChange == new Vector3(-2.4f, 0, 0))
                collision.transform.position = roomCenterObj.transform.position + new Vector3(13f, 0);
            else if (playerChange == new Vector3(0, 2.4f, 0))
                collision.transform.position = roomCenterObj.transform.position + new Vector3(0, -8.5f);
            else if (playerChange == new Vector3(0, -2.4f, 0))
                collision.transform.position = roomCenterObj.transform.position + new Vector3(0, 8.5f);
            //collision.transform.position += playerChange;

            gameObject.GetComponent<Sounds>().Play();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().lastTransfer = this.gameObject;
        }
    }

    private void Update()
    {
        if (enableOnEnter.activeInHierarchy && mainTransfer == true)
        {
            if (cam.currentRoom != roomCenterObj)
            {
                if (waitTime <= 0)
                {
                    waitTime = setWaitTime;
                    enableOnEnter.SetActive(false);
                    disableOnEnter.SetActive(true);
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }
}
