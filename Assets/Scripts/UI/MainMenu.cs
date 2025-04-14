using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject player;
    public GameObject karmaMeter;
    public GameObject map;
    public GameObject teleportBeam;
    private GameObject beam;
    private float waitTime = Mathf.Infinity;
    private float waitTime2 = Mathf.Infinity;
    public bool paused;
    public bool canPause = false;

    [Header("Main Menu")]
    public GameObject mainMenu;
    public GameObject rightPannel;
    public GameObject logo;
    public GameObject settingsMenu;
    public GameObject howToPlay;
    public GameObject controls;
    public GameObject abilities;

    [Header("Pause Menu")]
    public GameObject pauseMenu;
    public GameObject mainPauseMenu;
    public GameObject pauseSettingsMenu;
    public GameObject pauseHowToPlay;
    public GameObject pauseControls;
    public GameObject pauseAbilities;

    void Start()
    {
        Time.timeScale = 1;

        player.SetActive(false);
        mainMenu.SetActive(true);
        rightPannel.SetActive(true);
        settingsMenu.SetActive(false);
        howToPlay.SetActive(false);
        controls.SetActive(false);
        abilities.SetActive(false);
        karmaMeter.SetActive(false);
        map.SetActive(false);

        pauseMenu.SetActive(false);
        pauseSettingsMenu.SetActive(false);
        pauseHowToPlay.SetActive(false);
        pauseControls.SetActive(false);
        pauseAbilities.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
            if (canPause == true)
                if (paused == false)
                    Pause();
                else
                    PauseContinue();

        if (waitTime <= 0)
        {
            player.SetActive(true);
            canPause = true;
            waitTime = Mathf.Infinity;
        }
        if (waitTime2 <= 0)
        {
            Destroy(beam);
            player.GetComponent<PlayerMovement>().enabled = true;
            player.transform.Find("Fire Point").GetComponent<PlayerFiring>().enabled = true;
            waitTime2 = Mathf.Infinity;
        }
        else
        {
            waitTime -= Time.deltaTime;
            waitTime2 -= Time.deltaTime;
        }
    }


    public void Play()
    {
        rightPannel.GetComponent<Animator>().enabled = true;
        rightPannel.GetComponent<Animator>().SetBool("Move", true);
        logo.GetComponent<Animator>().SetBool("Move", true);
        karmaMeter.SetActive(true);
        map.SetActive(true);
        GameObject newBeam = Instantiate(teleportBeam, new Vector3 (0, 1, 0), teleportBeam.transform.rotation);
        beam = newBeam;
        waitTime = 0.3f;
        waitTime2 = 0.75f;
    }

    public void Settings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        howToPlay.SetActive(false);
    }

    public void HowToPlay()
    {
        mainMenu.SetActive(false);
        howToPlay.SetActive(true);
        controls.SetActive(false);
        abilities.SetActive(false);
    }

    public void Controls()
    {
        howToPlay.SetActive(false);
        controls.SetActive(true);
    }
    public void Abilities()
    {
        howToPlay.SetActive(false);
        abilities.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void BackToMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        howToPlay.SetActive(false);
        PlayerPrefs.Save();

        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Settings>().dungeonSizeChanged == true)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Settings>().dungeonSizeChanged = false;
            SceneManager.LoadScene(0);
        }
    }

    public void BackToHowToPlay()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        howToPlay.SetActive(true);
        controls.SetActive(false);
        abilities.SetActive(false);
    }




    public void Pause()
    {
        if (player != null)
        {
            pauseMenu.SetActive(true);
            karmaMeter.SetActive(false);
            map.SetActive(false);
            Time.timeScale = 0;
            paused = true;
        }
    }

    public void PauseContinue()
    {
        pauseMenu.SetActive(false);
        karmaMeter.SetActive(true);
        map.SetActive(true);
        Time.timeScale = 1;
        paused = false;
    }

    public void PauseSettings()
    {
        mainPauseMenu.SetActive(false);
        pauseSettingsMenu.SetActive(true);
        pauseHowToPlay.SetActive(false);
    }

    public void PauseHowToPlay()
    {
        mainPauseMenu.SetActive(false);
        pauseHowToPlay.SetActive(true);
        pauseControls.SetActive(false);
        pauseAbilities.SetActive(false);
    }

    public void PauseControls()
    {
        pauseHowToPlay.SetActive(false);
        pauseControls.SetActive(true);
    }
    public void PauseAbilities()
    {
        pauseHowToPlay.SetActive(false);
        pauseAbilities.SetActive(true);
    }

    public void PauseQuit()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseBackToMenu()
    {
        mainPauseMenu.SetActive(true);
        pauseSettingsMenu.SetActive(false);
        pauseHowToPlay.SetActive(false);
        PlayerPrefs.Save();
    }

    public void PauseBackToHowToPlay()
    {
        mainPauseMenu.SetActive(false);
        pauseSettingsMenu.SetActive(false);
        pauseHowToPlay.SetActive(true);
        pauseControls.SetActive(false);
        pauseAbilities.SetActive(false);
    }
}
