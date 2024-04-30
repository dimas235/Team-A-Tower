using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject pausePanel; // Reference to the pause panel
    public GameObject gameUI; // Reference to the in-game UI

    private bool isPaused = false; // To track the pause state

    void Start()
    {
        // Ensure the pause panel is not active when the game starts
        pausePanel.SetActive(false);
    }

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the pause state
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0f; // Stops the game time
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f; // Resumes normal game time
        isPaused = false;
    }

    // Function to return to the Main Menu
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Ensure time is back to normal
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your main menu scene name
    }
}
