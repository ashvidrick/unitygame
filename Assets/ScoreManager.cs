using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Текущий счёт")]
    public int score = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public static void AddScore(int amount)
    {
        if (Instance == null)
        {
            Debug.LogWarning("ScoreManager не найден в сцене!");
            return;
        }

        Instance.score += amount;
        Debug.Log($"Score: {Instance.score}");
    }

    public static void ResetScore()
    {
        if (Instance != null) Instance.score = 0;
    }
}