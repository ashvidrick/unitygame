using UnityEngine;

[ExecuteAlways]
public class Rotator : MonoBehaviour
{
    [Header("Вращение")]
    public Vector3 rotationAxis = new Vector3(0f, 1f, 0f);   // ось Y
    public float rotationSpeed = 90f;                        // градусов в секунду

    [Header("Направление")]
    public bool clockwise = true;

    void Update()
    {
        float dir = clockwise ? 1f : -1f;
        transform.Rotate(rotationAxis * rotationSpeed * dir * Time.deltaTime, Space.Self);
    }
}