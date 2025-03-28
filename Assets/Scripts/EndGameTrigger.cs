using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameTrigger : MonoBehaviour {
    public GameObject endPanel;
    public Button exitButton;
    public Button replayButton;
    public Button nextButton;

    private bool isLevelEnded = false;

    public Transform player;
    public Transform endPoint;
    public float activationRadius = 5f;

    public int currentLevelIndex;
    private SaveManager saveManager;

    void Start() {
        exitButton.onClick.AddListener(ExitToMainMenu);
        replayButton.onClick.AddListener(RestartLevel);
        nextButton.onClick.AddListener(NextLevel);
        saveManager = FindObjectOfType<SaveManager>();
        if (saveManager == null) {
            Debug.LogError("SaveManager не найден!");
        }
    }

    void Update() {
        if (!isLevelEnded && Vector3.Distance(player.position, endPoint.position) <= activationRadius) {
            isLevelEnded = true;
            if (saveManager != null) {
                if (!saveManager.CurrentSaveData.completedLevels.Contains(currentLevelIndex)) {
                    saveManager.CurrentSaveData.completedLevels.Add(currentLevelIndex);
                    saveManager.SaveGame();
                    Debug.Log("Уровень " + currentLevelIndex + " пройден и сохранён.");
                }
            }
            ShowEndPanel();
        }
    }

    void ShowEndPanel() {
        Time.timeScale = 0f;
        endPanel.SetActive(true);
        CanvasGroup canvasGroup = endPanel.GetComponent<CanvasGroup>();
        if (canvasGroup != null) {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    void ExitToMainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    void RestartLevel() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void NextLevel() {
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}
