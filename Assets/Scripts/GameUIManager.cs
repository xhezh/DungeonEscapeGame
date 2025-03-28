using UnityEngine;
using TMPro;

public class GameUIManager : MonoBehaviour {
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI waveText;

    private int gold = 0;
    private int currentWave = 1;

    public void UpdateGold(int amount) {
        gold = amount;
        goldText.text = "Gold: " + gold;
    }

    public void UpdateHealth(int health) {
        healthText.text = "Health: " + health;
    }

    public void UpdateWave(int wave, int total) {
        currentWave = wave;
        waveText.text = "Wave: " + currentWave + "/" + total;
    }
}
