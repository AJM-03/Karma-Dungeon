using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCursor : MonoBehaviour
{
    void Start()
    {
        //Cursor.visible = false;
    }

    void Update()
    {
        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Settings>().customCursorSetting == true)
        {
            gameObject.GetComponent<Image>().enabled = true;
            Vector2 cursorPos = Input.mousePosition;
            transform.position = cursorPos;

            if (Cursor.visible == true)
                Cursor.visible = false;
        }
        else
        {
            gameObject.GetComponent<Image>().enabled = false;
            if (Cursor.visible == false)
                Cursor.visible = true;
        }
    }
}
