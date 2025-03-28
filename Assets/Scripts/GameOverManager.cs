using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {
    public GameObject gameOverCanvas;

    void Start() {
        gameOverCanvas.SetActive(false);
    }

    public void ShowGameOver() {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0f;

        PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();
        if (pauseMenu != null) {
            pauseMenu.enabled = false;
        }
    }

    public void ReplayLevel() {
        PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();
        if (pauseMenu != null) {
            pauseMenu.ResumeGame();
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
