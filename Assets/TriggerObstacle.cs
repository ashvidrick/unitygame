using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerObstacle : MonoBehaviour
{
    [Header("Эффект")]
    public GameObject explosionFx;
    public float explosionLifetime = 2f;

    [Header("Счёт")]
    public int scoreValue = (-10);

    [Header("Отладка")]
    public bool showDebug = false;

    void OnTriggerEnter(Collider other)
    {
        var motor = other.GetComponentInParent<VitorcMotor>();
        if (motor == null) return;

        if (showDebug) Debug.Log($"TriggerObstacle HIT! +{scoreValue}");

        // 1. счёт
        ScoreManager.AddScore(scoreValue);

        // 2. эффект
        if (explosionFx != null)
        {
            var fx = Instantiate(explosionFx, transform.position, Quaternion.identity);
            Destroy(fx, explosionLifetime);
        }

        // 3. удалить себя
        Destroy(gameObject);
    }
}