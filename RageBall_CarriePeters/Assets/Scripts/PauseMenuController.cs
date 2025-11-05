using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenu;
    public Slider volumeSlider;      // single slider

    [Header("Audio Sources")]
    public List<AudioSource> audioSources = new List<AudioSource>();

    [Header("Settings")]
    [Range(0f, 1f)]
    public float defaultVolume = 0.5f;   // start at 50%

    bool isPaused = false;

    void Start()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        Time.timeScale = 1f;

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = defaultVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // push the default volume to all audio sources at start
        SetVolume(defaultVolume);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    void SetVolume(float value)
    {
        foreach (var src in audioSources)
        {
            if (src != null)
                src.volume = value;
        }
    }
}
