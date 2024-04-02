using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseController : MonoBehaviour
{   
    public GameObject panelLose; // Reference ke panel lose
    public TowerHealthDefens towerHealth; // Reference ke health tower
    public GameObject gameUI; // Reference ke UI in-game
    public GameObject pauseUI; // Reference ke panel pause

    void Start()
    {
        panelLose.SetActive(false);
    }

    public void ActivateLosePanel()
    {
        Time.timeScale = 0f;
        panelLose.SetActive(true);
        gameUI.SetActive(false);
        pauseUI.SetActive(false);
    }

    // Fungsi untuk mengulang game
    public void TryAgain()
    {
        Time.timeScale = 1f; // Pastikan waktu kembali normal

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Fungsi untuk kembali ke MainMenu
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // Method untuk menangani tower yang dihancurkan
    public void HandleTowerDestroyed()
    {
        // Tampilkan panel lose setelah delay untuk mensimulasikan waktu animasi
        Invoke(nameof(ActivateLosePanel), 1.0f); // Ganti 1.0f dengan durasi animasi hancur yang sesungguhnya
    }
}
