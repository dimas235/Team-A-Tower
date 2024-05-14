using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{
    public GameObject panelWin; // Reference ke panel win
    public GameObject gameUI; // Reference ke UI in-game
    public GameObject pauseUI; // Reference ke panel pause

    // Update is called once per frame
    void Start()
    {
        panelWin.SetActive(false);
    }

    public void ActivateWinPanel()
    {
        Time.timeScale = 0f;
        panelWin.SetActive(true);
        gameUI.SetActive(false);
        pauseUI.SetActive(false);
    }

    // Fungsi untuk kembali ke MainMenu
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void TryAgain()
    {
        Time.timeScale = 1f; // Pastikan waktu kembali normal

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level2");
    }

    // Method untuk menangani tower yang dihancurkan
    public void HandleTowerDestroyed()
    {
        // Tampilkan panel win setelah delay untuk mensimulasikan waktu animasi
        Invoke(nameof(ActivateWinPanel), 1.0f); // Ganti 1.0f dengan durasi animasi hancur yang sesungguhnya
    }


}
