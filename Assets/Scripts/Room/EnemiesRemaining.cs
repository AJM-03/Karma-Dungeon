using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesRemaining : MonoBehaviour
{
    private float totalKarma;
    private Karma karmaScript;
    private GameObject player;

    public float roomKarmaRefresh = 15f;
    private float countdown = 0;
    private int done = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            totalKarma = totalKarma + enemy.goodKarmaValue;
            if (done == 2)
                done = 3;
        }

        else if (collision.gameObject == player && done == 1)
            done = 2;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            totalKarma = totalKarma - enemy.goodKarmaValue;
        }

        else if (collision.gameObject == player)
        {
            if (countdown == 0f && player != null)
            {
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().form != "Soul")
                {
                    karmaScript = player.GetComponent<Karma>();
                    karmaScript.KarmaAdd(totalKarma);
                    countdown = roomKarmaRefresh;
                }
            }
        }
    }

    private void Update()
    {
        countdown -= Time.deltaTime;  // Counts down
        countdown = Mathf.Clamp(countdown, 0, roomKarmaRefresh);

        if (player == null && GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (totalKarma == 0 && done == 3 && player != null)
        {
            player.gameObject.GetComponent<PlayerMovement>().Speak(2);
            done = 4;
        }
    }
}