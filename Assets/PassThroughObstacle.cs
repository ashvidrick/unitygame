using UnityEngine;
using UnityEngine.SceneManagement;

public class PassThroughObstacle : MonoBehaviour
{
    [Header("Счёт")]
    public int scoreValue = 1;

    [Header("Отладка")]
    public bool showDebug = false;

    void OnTriggerEnter(Collider other)
    {
        var motor = other.GetComponentInParent<VitorcMotor>();
        if (motor == null) return;

        if (showDebug) Debug.Log($"PassThrough HIT! +{scoreValue}");

        // счёт
        ScoreManager.AddScore(scoreValue);

        // просто исчезает — без эффекта
        Destroy(gameObject);
    }
}