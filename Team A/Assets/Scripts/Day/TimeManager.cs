using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public enum TimeOfDay
    {
        Day,
        Night
    }

    public TimeOfDay currentTimeOfDay = TimeOfDay.Day;

    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private Texture2D skyboxNight;
    [SerializeField] private Texture2D skyboxDay;
    [SerializeField] private Gradient gradientDayToNight;
    [SerializeField] private Gradient gradientNightToDay;
    [SerializeField] private Light globalLight;

    public float changeIntervalInSeconds = 60f;
    private float timeSinceLastChange = 0f;

    public event System.Action OnTimeChange;

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
        ResetToDaytime();
        StartCoroutine(UpdateGameTime());
    }

    private void ResetToDaytime()
    {
        skyboxMaterial.SetTexture("_Texture1", skyboxDay);
        skyboxMaterial.SetTexture("_Texture2", skyboxNight);
        skyboxMaterial.SetFloat("_Blend", 0);
        globalLight.color = gradientDayToNight.Evaluate(0);
        RenderSettings.fogColor = globalLight.color;
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
                HandleTimeChange();
                timeSinceLastChange = 0f;
            }
        }
    }

    private void HandleTimeChange()
    {
        currentTimeOfDay = (currentTimeOfDay == TimeOfDay.Day) ? TimeOfDay.Night : TimeOfDay.Day;
        OnTimeChange?.Invoke();

        if (currentTimeOfDay == TimeOfDay.Day)
        {
            StartCoroutine(LerpSkybox(1f, 0f, 5f));
            StartCoroutine(LerpLight(gradientNightToDay, 5f));
        }
        else
        {
            StartCoroutine(LerpSkybox(0f, 1f, 5f));
            StartCoroutine(LerpLight(gradientDayToNight, 5f));
        }
    }

    private IEnumerator LerpSkybox(float startBlend, float endBlend, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float blendValue = Mathf.Lerp(startBlend, endBlend, time / duration);
            skyboxMaterial.SetFloat("_Blend", blendValue);
            yield return null;
        }

        skyboxMaterial.SetFloat("_Blend", endBlend);
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
        globalLight.color = gradient.Evaluate(1f);
        RenderSettings.fogColor = globalLight.color;
    }
}
