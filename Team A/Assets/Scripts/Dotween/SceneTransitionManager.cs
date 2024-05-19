using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    public CanvasGroup transitionCanvasGroup;
    public float transitionDuration = 1.0f; // Durasi transisi

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Pastikan canvas group pada alpha 0 pada awalnya (tidak terlihat)
        transitionCanvasGroup.alpha = 0;
    }

    public void TransitionToScene(string sceneName)
    {
        // Zoom in panel dengan mengubah alpha dari 0 ke 1
        transitionCanvasGroup.DOFade(1, transitionDuration).OnComplete(() =>
        {
            // Muat scene baru setelah transisi selesai
            SceneManager.LoadScene(sceneName);
        });
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Set alpha ke 1 dan kemudian fade out ke 0
        transitionCanvasGroup.alpha = 1;
        transitionCanvasGroup.DOFade(0, transitionDuration);
    }

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
