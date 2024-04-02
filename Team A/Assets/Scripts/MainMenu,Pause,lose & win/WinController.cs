using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{
    public GameObject panelWin; // Reference ke panel win
    public GameObject gameUI; // Reference ke UI in-game
    public GameObject pauseUI; // Reference ke panel pause
    public TowerHealthAttacker towerHealth; // Reference ke health tower

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

    // public void NextLevel()
    // {
    //     Time.timeScale = 1f; // Pastikan waktu kembali normal

    //     // Ganti "Level2" dengan nama scene level selanjutnya
    //     SceneManager.LoadScene("Level2");
    // }

    // Fungsi untuk kembali ke MainMenu
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // Method untuk menangani tower yang dihancurkan
    public void HandleTowerDestroyed()
    {
        // Tampilkan panel win setelah delay untuk mensimulasikan waktu animasi
        Invoke(nameof(ActivateWinPanel), 1.0f); // Ganti 1.0f dengan durasi animasi hancur yang sesungguhnya
    }


}
