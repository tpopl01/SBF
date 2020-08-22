using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    Transform pauseMenu;
    Transform settingsMenu;

    //Audio Settings
    public AudioMixer audioMixer;
    //Resolution Settings
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;

    public bool InMenu { get; set; }

    private void Start()
    {
        pauseMenu = transform.GetChild(0);
        settingsMenu = transform.GetChild(1);

        //Resolution
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int curResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                curResIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = curResIndex;
        resolutionDropdown.RefreshShownValue();

        //pauseMenu.gameObject.SetActive(false);
       // settingsMenu.gameObject.SetActive(false);
        OpenCloseMenu();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OpenCloseMenu();
        }
    }

    public void OpenCloseMenu()
    {
        bool open = !pauseMenu.gameObject.activeSelf;
        pauseMenu.gameObject.SetActive(open);
        OpenSettings(false);

        if(open)
        {
            InMenu = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {
            InMenu = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }

    public void OpenCloseSettings()
    {
        settingsMenu.gameObject.SetActive(true);
    }

    public void OpenSettings(bool open)
    {
        settingsMenu.gameObject.SetActive(open);
    }

    public void ResumeGame()
    {
        OpenCloseMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetResolution(int resIndex)
    {
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }


    public static PauseMenu singleton;
    private void Awake()
    {
        singleton = this;
    }
}
