using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("OpeningVideo");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}