using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyAfter : MonoBehaviour
{
    public float destroyTime;
    private float waitTime;
    public bool endGame = false;

    void Start()
    {
        waitTime = destroyTime;
    }


    void Update()
    {
        if (waitTime <= 0)
        {
            if (endGame == true)
                SceneManager.LoadScene(0);
            else
                Destroy(gameObject);
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
