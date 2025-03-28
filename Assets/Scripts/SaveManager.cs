using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour {
    private string savePath;

    public SaveData CurrentSaveData { get; private set; }

    void Awake() {
        DontDestroyOnLoad(gameObject);
        savePath = Path.Combine(Application.persistentDataPath, "savefile.json");
        LoadGame();
    }

    public void SaveGame() {
        string json = JsonUtility.ToJson(CurrentSaveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved: " + savePath);
    }

    public void LoadGame() {
        if (File.Exists(savePath)) {
            string json = File.ReadAllText(savePath);
            CurrentSaveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded: " + json);
        }
        else {
            CurrentSaveData = new SaveData();
            SaveGame();
        }

        AudioListener.volume = CurrentSaveData.volume;
    }
}
