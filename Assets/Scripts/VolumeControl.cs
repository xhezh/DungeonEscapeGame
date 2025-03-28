using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour {
    public Slider volumeSlider;
    private SaveManager saveManager;

    void Start() {
        AudioListener.volume = 0.3f;
        saveManager = FindObjectOfType<SaveManager>();
        volumeSlider.value = AudioListener.volume;
    }

    public void OnVolumeChanged() {
        AudioListener.volume = volumeSlider.value;

        if (saveManager != null) {
            saveManager.CurrentSaveData.volume = volumeSlider.value;
            saveManager.SaveGame();
        }
    }
}
