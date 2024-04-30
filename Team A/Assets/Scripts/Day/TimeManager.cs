using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // Singleton pattern
    public static TimeManager Instance { get; private set; }

    // Define an enum to represent time of day
    public enum TimeOfDay
    {
        Day,
        Night
    }

    // Public variable to hold current time of day
    public TimeOfDay currentTimeOfDay = TimeOfDay.Day;

    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private Texture2D skyboxNight;
    [SerializeField] private Texture2D skyboxDay;
    [SerializeField] private Gradient gradientDayToNight;
    [SerializeField] private Gradient gradientNightToDay;
    [SerializeField] private Light globalLight;

    public float changeIntervalInSeconds = 60f;
    private float timeSinceLastChange = 0f;

    // Event untuk memberitahu perubahan waktu
    public event System.Action OnTimeChange;

    private void Awake()
    {
        // Singleton setup
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
        ResetToDaytime();
        StartCoroutine(UpdateGameTime());
    }

    private void ResetToDaytime()
    {
        // Set skybox to daytime texture
        skyboxMaterial.SetTexture("_MainTex", skyboxDay);
        skyboxMaterial.SetFloat("_Blend", 0); // Ensure it's full day texture
        globalLight.color = gradientDayToNight.Evaluate(0); // Set light to day
        RenderSettings.fogColor = globalLight.color; // Set fog to day
        currentTimeOfDay = TimeOfDay.Day;
    }

    private IEnumerator UpdateGameTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            timeSinceLastChange += 1f;

            if (timeSinceLastChange >= changeIntervalInSeconds)
            {
                HandleTimeChange(); // Panggil metode HandleTimeChange saat waktu berubah
                timeSinceLastChange = 0f;
            }
        }
    }

    private void HandleTimeChange()
    {
        // Toggle between day and night
        currentTimeOfDay = (currentTimeOfDay == TimeOfDay.Day) ? TimeOfDay.Night : TimeOfDay.Day;

        // Panggil event OnTimeChange untuk memberitahu perubahan waktu
        OnTimeChange?.Invoke();

        if (currentTimeOfDay == TimeOfDay.Day)
        {
            StartCoroutine(LerpSkybox(skyboxDay, skyboxNight, 5f));
            StartCoroutine(LerpLight(gradientDayToNight, 5f));
        }
        else
        {
            StartCoroutine(LerpSkybox(skyboxNight, skyboxDay, 5f));
            StartCoroutine(LerpLight(gradientNightToDay, 5f));
        }
    }

    private IEnumerator LerpSkybox(Texture2D startTexture, Texture2D endTexture, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float blendValue = time / duration;
            skyboxMaterial.SetFloat("_Blend", blendValue);
            yield return null;
        }
        // Ensure complete transition to end texture
        skyboxMaterial.SetFloat("_Blend", currentTimeOfDay == TimeOfDay.Day ? 0f : 1f);
    }

    private IEnumerator LerpLight(Gradient gradient, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            globalLight.color = gradient.Evaluate(time / duration);
            RenderSettings.fogColor = globalLight.color;
            yield return null;
        }
    }
}
