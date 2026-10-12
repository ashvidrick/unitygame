using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // сюда тащим Player
    public Vector3 offset = new Vector3(0f, 2.5f, -6f);
    public float lateralLag = 4f;     // плавность по X
    public bool followZ = false;     // по Z обычно НЕ следуем, чтобы игрок «убегал вглубь»

    private float startZ;

    void Start()
    {
        if (target == null) target = GameObject.FindWithTag("Player")?.transform;
        startZ = transform.position.z;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float targetX = target.position.x + offset.x;
        float newX = Mathf.Lerp(transform.position.x, targetX, lateralLag * Time.deltaTime);

        float newZ = followZ ? target.position.z + offset.z : startZ;

        transform.position = new Vector3(newX, offset.y, newZ);
    }
}