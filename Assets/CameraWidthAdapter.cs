using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraWidthAdapter : MonoBehaviour
{
    [Header("Желаемая ширина мира в юнитах")]
    public float targetWidth = 12f; // подгони под свой коридор (например, 10 или 12)

    private Camera cam;
    private int lastWidth, lastHeight;
    private ScreenOrientation lastOrientation;

    void Awake()
    {
        cam = GetComponent<Camera>();
        Apply();
    }

    /*void Update()
    {
        // Пересчитываем только при смене разрешения/ориентации
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            Apply();
        }
    }*/
    void Update()
    {
        if (Screen.orientation != lastOrientation)
        {
            lastOrientation = Screen.orientation;
            Apply(); // принудительно пересчитать
        }
    }

    void Apply()
    {
        lastWidth = Screen.width;
        lastHeight = Screen.height;

        // Для ортографической камеры (2D/2.5D)
        if (cam.orthographic)
        {
            // orthographicSize = половина высоты. Нам нужна ширина.
            // width = size * 2 * aspect  =>  size = width / (2 * aspect)
            float aspect = (float)Screen.width / Screen.height;
            cam.orthographicSize = targetWidth / (2f * aspect);
        }
        else
        {
            // Для перспективной камеры (твоя 3D)
            // Привязываем горизонтальный FOV
            float aspect = (float)Screen.width / Screen.height;
            // Желаемый горизонтальный угол (подбери, чтобы влезли фишки)
            float hFov = 80f;
            cam.fieldOfView = 2f * Mathf.Atan(Mathf.Tan(hFov * 0.5f * Mathf.Deg2Rad) / aspect) * Mathf.Rad2Deg;
        }
    }
}