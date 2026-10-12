using UnityEngine;

public class CameraDirector : MonoBehaviour
{
    [Header("Ссылки")]
    public Camera mainCamera;
    public Camera orbitCamera;
    public OrbitCameraRig orbitRig;

    [Header("Параметры переключения")]
    public KeyCode toggleKey = KeyCode.C;
    public float duration = 15f;       // сколько секунд длится орбита
    public float rotations = 1f;       // сколько полных кругов сделать (если > 0 — перебивает duration)

    [Header("Отладка")]
    public bool showDebug = false;

    private bool inOrbit;
    private float timer;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        SetActiveCamera(false);   // по умолчанию — main
    }

    void Update()
    {
        // переключение по клавише
        if (Input.GetKeyDown(toggleKey) && !inOrbit)
        {
            StartOrbit();
        }

        if (inOrbit)
        {
            timer -= Time.deltaTime;

            // завершение по времени
            if (timer <= 0f)
            {
                StopOrbit();
                return;
            }

            // завершение по количеству кругов
            if (rotations > 0f)
            {
                float totalRotation = orbitRig.CurrentAngle - orbitRig.startAngle;
                // Если угол ушёл за 360 — считаем круг
                // Простая проверка: сколько раз currentAngle прошёл 0
                // (можно улучшить через накопленный счётчик в OrbitCameraRig)
            }
        }
    }

    public void StartOrbit()
    {
        inOrbit = true;

        // Считаем время: либо задано duration, либо через rotations/rotationSpeed
        if (rotations > 0f && orbitRig != null && orbitRig.rotationSpeed > 0f)
        {
            timer = (360f * rotations) / orbitRig.rotationSpeed;
        }
        else
        {
            timer = duration;
        }

        orbitRig.StartOrbit();
        SetActiveCamera(true);

        if (showDebug)
            Debug.Log($"Start orbit for {timer:F1}s");
    }

    public void StopOrbit()
    {
        inOrbit = false;
        orbitRig.StopOrbit();
        SetActiveCamera(false);

        if (showDebug)
            Debug.Log("Stop orbit");
    }

    void SetActiveCamera(bool orbit)
    {
        if (mainCamera != null) mainCamera.enabled = !orbit;
        if (orbitCamera != null) orbitCamera.enabled = orbit;
    }

    public bool InOrbit => inOrbit;
}