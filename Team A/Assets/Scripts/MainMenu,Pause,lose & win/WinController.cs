using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening; // Tambahkan referensi ke DOTween

public class WinController : MonoBehaviour
{
    public GameObject panelWin; // Referensi ke panel win
    public GameObject gameUI; // Referensi ke UI in-game
    public GameObject pauseUI; // Referensi ke panel pause
    public RectTransform panelWinRect; // Referensi ke RectTransform panel win

    [SerializeField] float tweenDuration;

    void Start()
    {
        panelWin.SetActive(false);
        panelWinRect.localScale = Vector3.zero; // Set skala awal ke nol
    }

    public void ActivateWinPanel()
    {
        Time.timeScale = 0f;
        panelWin.SetActive(true);
        gameUI.SetActive(false);
        pauseUI.SetActive(false);
        AnimateWinPanel();
        AudioManager.instance.PlayWinSFX(); // Mainkan SFX menang
    }

    void AnimateWinPanel()
    {
        // Menggabungkan animasi skala dan posisi (jika diperlukan)
        panelWinRect.DOScale(Vector3.one, tweenDuration).SetEase(Ease.OutBack).SetUpdate(true);
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

    // Metode untuk menangani tower yang dihancurkan
    public void HandleTowerDestroyed()
    {
        // Tampilkan panel win setelah delay untuk mensimulasikan waktu animasi
        Invoke(nameof(ActivateWinPanel), 1.0f); // Ganti 1.0f dengan durasi animasi hancur yang sesungguhnya
    }
}
