using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [Header("Ссылки")]
    public ScoreManager scoreManager;
    public TextMeshProUGUI scoreText;

    void Update()
    {
        if (scoreManager == null || scoreText == null) return;
        scoreText.text = "Score: " + scoreManager.score;
    }
}