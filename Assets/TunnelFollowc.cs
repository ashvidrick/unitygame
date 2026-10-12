using UnityEngine;

public class TunnelFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 6f, 15f);

    void LateUpdate()
    {
        if (player == null) return;
        transform.position = player.position + offset;
    }
}