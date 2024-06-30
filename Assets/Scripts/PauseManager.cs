﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public Canvas pauseCanvas; // Reference to the Canvas containing your pause UI
    public Canvas minimapCanvas;
    public Canvas gameTimerCanvas;
    public GameTimer gameTimer;

    public static bool isPaused = false;

    void Start()
    {
        //pauseCanvas = GetComponent<Canvas>();
        pauseCanvas = GameObject.Find("PauseCanvas").GetComponent<Canvas>();
        minimapCanvas = GameObject.Find("MinimapView").GetComponent<Canvas>();
        gameTimerCanvas = GameObject.Find("GameTimer").GetComponent<Canvas>();
        gameTimer = FindObjectOfType<GameTimer>();
        // Hide the canvas at the start
        if (pauseCanvas != null)
        {
            pauseCanvas.enabled = false;
            minimapCanvas.enabled = true;
            gameTimerCanvas.enabled = true;
        }
        else
        {
            Debug.LogError("Pause Canvas is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        // Check if the player presses the pause key (e.g., Escape)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause(); // Toggle pause state
        }
    }

    public void SetPause()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pause the game

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // Show the mouse pointer

        if (pauseCanvas != null && minimapCanvas != null)
        {
            pauseCanvas.enabled = true; // Show the pause UI
            minimapCanvas.enabled = false;
            gameTimerCanvas.enabled= false;

            if(QuizManager.Instance != null)
            {
                if (QuizManager.quizStarted)
                {
                    QuizManager.Instance.quizCanvas.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("Pause Canvas is not assigned in the Inspector!");
        }
        gameTimer.StopTimer();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume normal time scale
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // Hide the mouse pointer

        if (pauseCanvas != null && minimapCanvas != null)
        {
            pauseCanvas.enabled = false; // Hide the pause UI
            minimapCanvas.enabled = true;
            gameTimerCanvas.enabled = true;

            if (QuizManager.Instance != null)
            {
                if (QuizManager.quizStarted)
                {
                    Time.timeScale = 0f; // Pause the game
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true; // Show the mouse pointer

                    QuizManager.Instance.quizCanvas.SetActive(true);
                }
            }
        }
        else
        {
            Debug.LogError("Pause Canvas is not assigned in the Inspector!");
        }

        gameTimer.StartTimer();
    }

    public void ExitMenu()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume normal time scale

        // Get all game objects in the scene, including those marked as DontDestroyOnLoad
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Destroy all game objects
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("MainCamera") || obj.CompareTag("Persistent"))
                continue;

            Destroy(obj);
        }

        // Load the menu scene
        SceneManager.LoadScene("InitializationScene");
    }

    // Function to toggle pause state and show/hide the pause UI
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            SetPause();
        }
    }
}
