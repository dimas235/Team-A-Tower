using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Clips")]
    public AudioClip loseSFX; // SFX untuk aktivasi panel kalah
    public AudioClip winSFX;  // SFX untuk aktivasi panel menang

    private AudioSource audioSource;

    void Awake()
    {
        // Pola Singleton untuk memastikan hanya ada satu instance AudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Metode untuk memainkan AudioClip tertentu
    public void PlaySFX(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Metode untuk memainkan SFX kalah
    public void PlayLoseSFX()
    {
        PlaySFX(loseSFX);
    }

    // Metode untuk memainkan SFX menang
    public void PlayWinSFX()
    {
        PlaySFX(winSFX);
    }
}
