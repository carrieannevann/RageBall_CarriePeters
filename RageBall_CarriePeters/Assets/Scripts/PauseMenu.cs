using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag the 'Container' child under PauseMenu here.")]
    public GameObject container;
    [Tooltip("Drag the Resume Button (its Button component) here.")]
    public Button resumeButton;

    private bool isPaused = false;

    void Awake()
    {
        // Hide the pause UI at start
        if (container != null) container.SetActive(false);

        // Wire the Resume button
        if (resumeButton != null)
            resumeButton.onClick.AddListener(Resume);
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
        if (container != null) container.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Focus the Resume button for keyboard/controller users
        if (resumeButton != null)
            EventSystem.current?.SetSelectedGameObject(resumeButton.gameObject);
    }

    public void Resume()
    {
        if (container != null) container.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        EventSystem.current?.SetSelectedGameObject(null);
    }

    void OnDisable()
    {
        // Safety: never leave Time.timeScale at 0 if this component is disabled
        if (isPaused) Time.timeScale = 1f;
    }
}
