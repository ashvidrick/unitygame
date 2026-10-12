using UnityEngine;

public class OrbitCameraRig : MonoBehaviour
{
    [Header("Ссылки")]
    public Transform player;
    public Transform orbitCamera;   // дочерняя камера

    [Header("Форма орбиты")]
    [Tooltip("Радиус по X (вправо-влево)")]
    public float radiusX = 8f;

    [Tooltip("Радиус по Z (вперёд-назад)")]
    public float radiusZ = 6f;

    [Tooltip("Насколько выше игрока летает камера")]
    public float height = 3f;

    [Header("Вращение")]
    [Tooltip("Скорость вращения (градусов в секунду)")]
    public float rotationSpeed = 90f;

    [Tooltip("Начальный угол (0 = сзади, 90 = справа, 180 = спереди, 270 = слева)")]
    public float startAngle = 0f;

    [Header("Слежение за игроком")]
    [Tooltip("Насколько выше центра смотреть (0 — центр игрока, 1 — примерно голова)")]
    public float lookAtHeight = 1f;

    [Header("Отладка")]
    public bool showDebug = false;

    private float currentAngle;
    private bool isRunning;

    void Start()
    {
        currentAngle = startAngle;
        isRunning = false;
        if (orbitCamera == null && transform.childCount > 0)
            orbitCamera = transform.GetChild(0);
    }

    void LateUpdate()
    {
        if (player == null || orbitCamera == null) return;

        // pivot всегда следует за игроком
        transform.position = player.position;

        if (!isRunning) return;

        // крутим угол
        currentAngle += rotationSpeed * Time.deltaTime;
        if (currentAngle >= 360f) currentAngle -= 360f;

        // считаем позицию на эллипсе
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(
            Mathf.Sin(rad) * radiusX,
            height,
            Mathf.Cos(rad) * radiusZ
        );

        // ставим камеру
        orbitCamera.localPosition = offset;

        // камера смотрит на игрока (с учётом высоты взгляда)
        Vector3 lookTarget = player.position + Vector3.up * lookAtHeight;
        orbitCamera.LookAt(lookTarget);

        if (showDebug)
            Debug.Log($"Orbit angle = {currentAngle:F1}, camPos = {orbitCamera.position}");
    }

    public void StartOrbit()
    {
        isRunning = true;
        currentAngle = startAngle;
    }

    public void StopOrbit()
    {
        isRunning = false;
    }

    public float CurrentAngle => currentAngle;
}