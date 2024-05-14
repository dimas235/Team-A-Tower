using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoseController : MonoBehaviour
{   
    public GameObject panelLose; // Reference ke panel lose
    public TowerHealthDefens towerHealth; // Reference ke health tower
    public GameObject gameUI; // Reference ke UI in-game
    public GameObject pauseUI; // Reference ke panel pause
    public RectTransform panelLoseRect; // Reference ke RectTransform panel lose

    [SerializeField] float tweenDuration = 0.5f; // Durasi animasi

    void Start()
    {
        panelLose.SetActive(false);
        panelLoseRect.localScale = Vector3.zero; // Set initial scale to zero
    }

    public void ActivateLosePanel()
    {
        Time.timeScale = 0f;
        panelLose.SetActive(true);
        gameUI.SetActive(false);
        pauseUI.SetActive(false);
        AnimateLosePanel();
    }

    void AnimateLosePanel()
    {
        // Menggabungkan animasi skala dan posisi (jika diperlukan)
        panelLoseRect.DOScale(Vector3.one, tweenDuration).SetEase(Ease.OutBack).SetUpdate(true);
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
