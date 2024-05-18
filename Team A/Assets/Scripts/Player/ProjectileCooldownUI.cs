using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileCooldownUI : MonoBehaviour
{
    public Image cooldownOverlay; // Referensi ke Image UI untuk overlay cooldown
    public Image background; // Referensi ke Image UI untuk latar belakang
    public float skillCooldown = 5.0f; // Waktu cooldown skill
    public bool isCooldown = false; // Status apakah sedang cooldown

    protected virtual void Awake()
    {
        cooldownOverlay.fillAmount = 0;
        background.gameObject.SetActive(true); // Tampilkan latar belakang pada awalnya
        cooldownOverlay.gameObject.SetActive(false); // Sembunyikan overlay pada awalnya
    }

    void Update()
    {
        // Mulai cooldown jika tombol R ditekan dan tidak sedang cooldown
        if (Input.GetKeyDown(KeyCode.R) && !isCooldown)
        {
            StartCoroutine(CooldownRoutine());
        }
    }

    public IEnumerator CooldownRoutine()
    {
        isCooldown = true;
        cooldownOverlay.gameObject.SetActive(true); // Tampilkan overlay saat cooldown
        background.gameObject.SetActive(false); // Sembunyikan latar belakang saat cooldown

        for (float i = 0; i < skillCooldown; i += Time.deltaTime)
        {
            cooldownOverlay.fillAmount = i / skillCooldown;
            yield return null;
        }

        cooldownOverlay.fillAmount = 0;
        cooldownOverlay.gameObject.SetActive(false); // Sembunyikan overlay setelah cooldown selesai
        background.gameObject.SetActive(true); // Tampilkan latar belakang setelah cooldown selesai
        isCooldown = false;
    }
}
