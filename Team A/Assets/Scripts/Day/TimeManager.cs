using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Texture2D skyboxNight;
    [SerializeField] private Texture2D skyboxDay;
    [SerializeField] private Gradient gradientDayToNight;
    [SerializeField] private Gradient gradientNightToDay;
    [SerializeField] private Light globalLight;

    public float changeIntervalInSeconds = 60f;

    private bool isDaytime = true; // Menambahkan variabel untuk menandai siang atau malam
    private float timeSinceLastChange = 0f;

    private void Start()
    {
        StartCoroutine(UpdateGameTime());
    }

    private IEnumerator UpdateGameTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            timeSinceLastChange += 1f;

            if (timeSinceLastChange >= changeIntervalInSeconds)
            {
                OnTimeChange();
                timeSinceLastChange = 0f;
                isDaytime = !isDaytime; // Mengubah status siang/malam setiap kali mengubah waktu
            }
        }
    }

    private void OnTimeChange()
    {
        if (isDaytime)
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

    private IEnumerator LerpSkybox(Texture2D a, Texture2D b, float time)
    {
        RenderSettings.skybox.SetTexture("_Texture1", a);
        RenderSettings.skybox.SetTexture("_Texture2", b);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", i / time);
            yield return null;
        }
        RenderSettings.skybox.SetTexture("_Texture1", b);
    }

    private IEnumerator LerpLight(Gradient lightGradient, float time)
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            globalLight.color = lightGradient.Evaluate(i / time);
            RenderSettings.fogColor = globalLight.color;
            yield return null;
        }
    }
}
