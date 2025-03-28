using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject levelSelectMenu;

    public void OpenMainMenu() {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        levelSelectMenu.SetActive(false);
    }

    public void OpenOptions() {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        levelSelectMenu.SetActive(false);
    }

    public void OpenLevelSelect() {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        levelSelectMenu.SetActive(true);
    }

    public void LoadLevel(int levelIndex) {
        SceneManager.LoadScene(levelIndex);
    }

    public void QuitGame() {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
