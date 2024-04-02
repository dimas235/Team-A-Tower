using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject pausePanel; // Reference ke panel pause
    public GameObject gameUI; // Reference ke UI in-game

    void Start()
    {
        // Pastikan panel pause tidak aktif ketika game dimulai
        pausePanel.SetActive(false);
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0f; // Menghentikan waktu di game
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f; // Mengembalikan waktu ke normal
    }

    // Fungsi untuk kembali ke MainMenu
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Pastikan waktu kembali normal
        SceneManager.LoadScene("MainMenu"); // Ganti "MainMenu" dengan nama scene menu utama kamu
    }
}
