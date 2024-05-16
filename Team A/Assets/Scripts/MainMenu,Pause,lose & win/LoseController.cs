using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LoseController : MonoBehaviour
{   
    public GameObject panelLose; // Referensi ke panel kalah
    public TowerHealthDefens towerHealth; // Referensi ke health tower
    public GameObject gameUI; // Referensi ke UI dalam game
    public GameObject pauseUI; // Referensi ke panel pause
    public RectTransform panelLoseRect; // Referensi ke RectTransform panel kalah

    [SerializeField] float tweenDuration = 0.5f; // Durasi animasi

    void Start()
    {
        panelLose.SetActive(false);
        panelLoseRect.localScale = Vector3.zero; // Set skala awal ke nol
    }

    public void ActivateLosePanel()
    {
        Time.timeScale = 0f;
        panelLose.SetActive(true);
        gameUI.SetActive(false);
        pauseUI.SetActive(false);
        AnimateLosePanel();
        AudioManager.instance.PlayLoseSFX(); // Mainkan SFX kalah
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

    // Metode untuk menangani tower yang dihancurkan
    public void HandleTowerDestroyed()
    {
        // Tampilkan panel kalah setelah delay untuk mensimulasikan waktu animasi
        Invoke(nameof(ActivateLosePanel), 1.0f); // Ganti 1.0f dengan durasi animasi hancur yang sesungguhnya
    }
}
