using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public float volumeSetting;
    public float musicSetting;
    public bool bossOnMapSetting;
    public bool customCursorSetting;
    public float dungeonSize;  // Reload Game when changed
    public bool dungeonSizeChanged = false;

    private float volume;
    private float volumeSet;
    public GameObject mainMusicAudioSource;
    public GameObject bossMusicAudioSource;
    public AudioSource soulSoundAudioSource;
    public AudioSource soulSparkleAudioSource;

    public Slider volumeSlider;
    public Slider musicVolumeSlider;
    public Slider dungeonSizeSlider;
    public Toggle customCursorToggle;
    public Toggle bossOnMapToggle;

    public Slider pauseVolumeSlider;
    public Slider pauseMusicVolumeSlider;
    public Toggle pauseCustomCursorToggle;

    void Start()
    {
        PlayMusic(1);
        LoadData();
        dungeonSizeChanged = false;


        volumeSetting = (PlayerPrefs.GetFloat("SoundEffectVolume"));
        musicSetting = (PlayerPrefs.GetFloat("MusicVolume"));
    }

    void Update()
    {
        volume = 1f;
        volumeSet = volume / 100;
        volume = volumeSet * musicSetting;
        mainMusicAudioSource.GetComponent<AudioSource>().volume = volume;
        bossMusicAudioSource.GetComponent<AudioSource>().volume = volume / 4;

        if (soulSoundAudioSource)
        {
            volume = 1f;
            volumeSet = volume / 100;
            volume = volumeSet * volumeSetting;
            soulSoundAudioSource.volume = volume;
            soulSparkleAudioSource.volume = volume;
        }
    }


    void LoadData()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("SoundEffectVolume", 100f);  // Sets the settings to the correct positions and a default one if one hasn't been saved
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 100f);
        dungeonSizeSlider.value = PlayerPrefs.GetFloat("DungeonSize", 15f);

        if (PlayerPrefs.GetInt("BossOnMap", 1) == 1)
            bossOnMapToggle.isOn = true;
        else
            bossOnMapToggle.isOn = false;

        if (PlayerPrefs.GetInt("CustomCursor", 1) == 1)
        {
            customCursorSetting = true;
            customCursorToggle.isOn = true;
            pauseCustomCursorToggle.isOn = true;
        }
        else
        {
            customCursorSetting = false;
            customCursorToggle.isOn = false;
            pauseCustomCursorToggle.isOn = false;
        }

        pauseVolumeSlider.value = volumeSlider.value;
        pauseMusicVolumeSlider.value = musicVolumeSlider.value;
        volumeSetting = volumeSlider.value;
        musicSetting = musicVolumeSlider.value;
    }


    private void OnDisable()
    {
        PlayerPrefs.Save();
    }


    public void SetVolume(float sliderValue)
    {
        PlayerPrefs.SetFloat("SoundEffectVolume", sliderValue);
        volumeSetting = (PlayerPrefs.GetFloat("SoundEffectVolume"));
        volumeSlider.value = PlayerPrefs.GetFloat("SoundEffectVolume");
        pauseVolumeSlider.value = PlayerPrefs.GetFloat("SoundEffectVolume");
    }

    public void SetMusicVolume(float sliderValue)
    {
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);  // Saves the new value
        musicSetting = (PlayerPrefs.GetFloat("MusicVolume"));
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        pauseMusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void SetDungeonSize(float sliderValue)
    {
        dungeonSize = (sliderValue);
        PlayerPrefs.SetFloat("DungeonSize", sliderValue);
        dungeonSizeSlider.value = PlayerPrefs.GetFloat("DungeonSize");
        GameObject.FindGameObjectWithTag("Rooms").GetComponent<Templates>().maxRooms = PlayerPrefs.GetFloat("DungeonSize");
        dungeonSizeChanged = true;
    }

    public void SetCustomCursor()
    {
        if (customCursorToggle.isOn == true)  // If it is on
        {
            PlayerPrefs.SetInt("CustomCursor", 1);  // Save it
            customCursorSetting = true;  // Set the variable
            pauseCustomCursorToggle.isOn = true;
        }
        else  // If it isn't on
        {
            PlayerPrefs.SetInt("CustomCursor", 0);  // Save it
            customCursorSetting = false;  // Set the variable
            pauseCustomCursorToggle.isOn = false;
        }
    }

    public void PauseSetCustomCursor()
    {
        if (pauseCustomCursorToggle.isOn == true)  // If it is on
        {
            PlayerPrefs.SetInt("CustomCursor", 1);  // Save it
            customCursorSetting = true;  // Set the variable
            customCursorToggle.isOn = true;
        }
        else  // If it isn't on
        {
            PlayerPrefs.SetInt("CustomCursor", 0);  // Save it
            customCursorSetting = false;  // Set the variable
            customCursorToggle.isOn = false;
        }
    }

    public void SetBossOnMap()
    {
        if (bossOnMapToggle.isOn == true)  // If it is on
        {
            PlayerPrefs.SetInt("BossOnMap", 1);  // Save it
            bossOnMapSetting = true;  // Set the variable
        }
        else  // If it isn't on
        {
            PlayerPrefs.SetInt("BossOnMap", 0);  // Save it
            bossOnMapSetting = false;  // Set the variable
        }
    }

    public void PlayMusic(int song)
    {
        if (song == 0)
        {
            mainMusicAudioSource.SetActive(false);
            bossMusicAudioSource.SetActive(false);
        }
        else if (song == 1)
        {
            mainMusicAudioSource.SetActive(true);
            bossMusicAudioSource.SetActive(false);
        }
        else if (song == 2)
        {
            mainMusicAudioSource.SetActive(false);
            bossMusicAudioSource.SetActive(true);
        }
    }
}
