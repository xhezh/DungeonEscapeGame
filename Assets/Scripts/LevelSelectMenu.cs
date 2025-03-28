using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelSelectMenu : MonoBehaviour {
    public SaveManager saveManager;
    public Button[] levelButtons;

    void OnEnable() {
        UpdateLevelButtons();
    }

    void UpdateLevelButtons() {
        if (saveManager == null) {
            saveManager = FindObjectOfType<SaveManager>();
        }

        List<int> completedLevels = saveManager.CurrentSaveData.completedLevels;
        Debug.Log("Обновление меню уровней. Пройденные уровни: ");
        foreach (int lvl in completedLevels) {
            Debug.Log(lvl);
        }

        for (int i = 0; i < levelButtons.Length; i++) {
            if (i == 0) {
                levelButtons[i].interactable = true;
            }
            else {
                levelButtons[i].interactable = completedLevels.Contains(i - 1);
            }
        }
    }
}
