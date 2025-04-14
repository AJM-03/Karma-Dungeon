using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    public bool spawned = false;

    private void Update()
    {
        if (spawned == true)
        {
            gameObject.transform.parent = gameObject.transform.parent.parent.parent.parent.parent.Find("Disable On Enter").Find("Walls");
            spawned = false;
        }
    }
}
