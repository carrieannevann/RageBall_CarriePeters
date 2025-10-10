using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenuUI;    // assign PauseMenuPanel
    public Slider volumeSlider;       // assign VolumeSlider

    [Header("HUD (optional)")]
    [Tooltip("Assign HUD GameObjects that should be hidden while paused (e.g. HUD Canvas, CountText).")]
    public GameObject[] hudObjects;   // objects to disable when paused

    [Header("Audio")]
    public bool useAudioMixer = true; // toggle which method to use
    public AudioMixer masterMixer;    // assign MasterMixer (optional)
    public string mixerExposedParam = "MasterVolume"; // exposed param name

    private bool isPaused = false;
    private const string PREF_VOLUME = "MasterVolumePref";

    void Start()
    {
        // Ensure menu is hidden initially
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);

        // Load saved volume
        float saved = PlayerPrefs.GetFloat(PREF_VOLUME, 1f); // default 1 (full)
        if (volumeSlider != null)
        {
            volumeSlider.value = saved;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // apply volume right away
        SetVolume(saved);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        Debug.Log("PauseMenu: Pause() called.");
        Time.timeScale = 0f;

        // show pause UI
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            // make sure it's rendered on top of other siblings
            pauseMenuUI.transform.SetAsLastSibling();
        }

        // hide HUD objects so they don't intercept clicks
        if (hudObjects != null)
        {
            for (int i = 0; i < hudObjects.Length; i++)
            {
                if (hudObjects[i] != null) hudObjects[i].SetActive(false);
            }
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
    }

    public void Resume()
    {
        Debug.Log("PauseMenu: Resume() called.");
        Time.timeScale = 1f;

        // hide pause UI
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);

        // restore HUD objects
        if (hudObjects != null)
        {
            for (int i = 0; i < hudObjects.Length; i++)
            {
                if (hudObjects[i] != null) hudObjects[i].SetActive(true);
            }
        }

        Cursor.lockState = CursorLockMode.Locked; // or your normal lock state
        Cursor.visible = false;
        isPaused = false;
    }

    // Hook this to the slider's OnValueChanged (or it's already hooked via listener in Start)
    public void SetVolume(float sliderValue)
    {
        // sliderValue expected in range 0 - 1
        sliderValue = Mathf.Clamp(sliderValue, 0.0001f, 1f); // avoid log(0)

        if (useAudioMixer && masterMixer != null)
        {
            // convert linear 0..1 to decibels (-80 dB .. 0 dB)
            float dB = Mathf.Log10(sliderValue) * 20f; // 1 -> 0dB, 0.0001 -> -80dB
            masterMixer.SetFloat(mixerExposedParam, dB);
        }
        else
        {
            // Simple fallback (global volume)
            AudioListener.volume = sliderValue;
        }

        // save preference (raw slider value)
        PlayerPrefs.SetFloat(PREF_VOLUME, sliderValue);
        PlayerPrefs.Save();
    }

    // Called by Quit button
    public void QuitGame()
    {
        // do any cleanup you need here

        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
