using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject Stage;
    public GameObject Information;
    public GameObject Credit;
    public GameObject PopUp;
    public GameObject ImageTengah;
    public GameObject IsiControl;

    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void CreditMenu()
    {
        mainMenu.SetActive(false);
        Credit.SetActive(true);
        ImageTengah.SetActive(false);
    }
    

    public void BackToMainMenuFromCredit()
    {
        mainMenu.SetActive(true);
        ImageTengah.SetActive(true);
        Credit.SetActive(false);
        IsiControl.SetActive(false);
    }

    public void NextToInformation()
    {
        IsiControl.SetActive(false);
        Information.SetActive(true);
    }

    public void BackToFromInformation()
    {
        IsiControl.SetActive(true);
        Information.SetActive(false);
    }

    public void StageMenu()
    {
        mainMenu.SetActive(false);
        Stage.SetActive(true);
    }

    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        ImageTengah.SetActive(true);
        Stage.SetActive(false);
    }

    public void GuideMenu()
    {
        mainMenu.SetActive(false);
        ImageTengah.SetActive(false);
        IsiControl.SetActive(true);
    }

    public void BackToMainMenuFromGuide()
    {
        mainMenu.SetActive(true);
        ImageTengah.SetActive(true);
        IsiControl.SetActive(false);
    }

    public void PopUpMenu()
    {
        PopUp.SetActive(true);
    }

    public void ClosePopUp()
    {
        PopUp.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
