using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTeleporter : MonoBehaviour
{
    private float speed;
    private GameObject player;
    private GameObject beam;
    public float slowSpinSpeed;
    public float fastSpinSpeed;
    public GameObject teleportBeam;
    private float waitTime = 3;
    private float waitTime2 = 2.8f;
    private float waitTime3 = 3.55f;
    private float waitTime4 = 6;

    void Start()
    {
        speed = slowSpinSpeed;
    }

    void Update()
    {
        transform.Rotate(Vector3.back * speed * Time.deltaTime);

        if (waitTime <= 0)
        {
            player.SetActive(false);
            waitTime = Mathf.Infinity;
        }
        if (waitTime2 <= 0)
        {
            GameObject newBeam = Instantiate(teleportBeam, transform.position + new Vector3(0, 1, 0), teleportBeam.transform.rotation);
            beam = newBeam;
            beam.transform.parent = transform.parent;
            gameObject.GetComponent<Sounds>().Play();
            waitTime2 = Mathf.Infinity;
        }
        if (waitTime3 <= 0)
        {
            Destroy(beam);
            waitTime3 = Mathf.Infinity;
        }
        if (waitTime4 <= 0)
        {
            SceneManager.LoadScene(0);
            waitTime4 = Mathf.Infinity;
        }

        if (speed == fastSpinSpeed)
        {
            waitTime -= Time.deltaTime;
            waitTime2 -= Time.deltaTime;
            waitTime3 -= Time.deltaTime;
            waitTime4 -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            speed = fastSpinSpeed;
            player = collision.gameObject;
            player.transform.position = transform.position + new Vector3(0, 0.5f, 0);
            player.GetComponent<PlayerMovement>().playerAnim.SetBool("ArmsUp", true);
            player.GetComponent<PlayerMovement>().playerAnim.SetBool("Moving", false);
            player.GetComponent<PlayerMovement>().playerAnim.SetBool("Fire", false);
            player.GetComponent<PlayerMovement>().enabled = false;
        }
    }
}
