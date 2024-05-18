using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public GameObject panelMiniCamera; // Reference to the PanelMiniCamera

    private void Awake()
    {
        // Disable the panel at the start
        if (panelMiniCamera != null)
        {
            panelMiniCamera.SetActive(false);
        }
    }

    public void ActivateMiniCameraPanel()
    {
        if (panelMiniCamera != null)
        {
            panelMiniCamera.SetActive(true);
        }
    }

    public void DeactivateMiniCameraPanel()
    {
        if (panelMiniCamera != null)
        {
            panelMiniCamera.SetActive(false);
        }
    }

    public IEnumerator DeactivateMiniCameraPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DeactivateMiniCameraPanel();
        panelMiniCamera.SetActive(false);
    }
}
