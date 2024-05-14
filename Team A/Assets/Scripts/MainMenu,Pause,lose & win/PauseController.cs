using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Threading.Tasks; // Tambahkan ini untuk menggunakan Task

public class PauseController : MonoBehaviour
{
    public GameObject pausePanel; // Reference to the pause panel
    public GameObject gameUI; // Reference to the in-game UI
    public GameObject pause;
    
    [SerializeField] RectTransform pausePanelRect;
    [SerializeField] float topPosY, middlePosY;
    [SerializeField] float tweenDuration;

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

    public void RestartGame()
    {
        Time.timeScale = 1f; // Ensure time is back to normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public void PauseGame()
    {
        pause.SetActive(false);
        pausePanel.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0f; // Stops the game time
        isPaused = true;
        PausePanelIntro();
    }

    void PausePanelIntro()
    {
        pausePanelRect.DOAnchorPosY(middlePosY, tweenDuration).SetUpdate(true);
    }

    async Task PausePanelOutro()
    {
        await pausePanelRect.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }

    public async void ResumeGame()
    {
        await PausePanelOutro();
        pause.SetActive(true);
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
