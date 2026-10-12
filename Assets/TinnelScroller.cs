using UnityEngine;

public class TunnelScroller : MonoBehaviour
{
    [Header("Ссылки")]
    public Renderer tunnelRenderer;
    public VitorcMotor player;

    [Header("Настройки")]
    [Tooltip("Насколько быстро крутится текстура при скорости игрока")]
    public float factor = 0f;

    private Material mat;
    private float scrollOffset;

    void Start()
    {
        if (tunnelRenderer == null)
        {
            Debug.LogError("TunnelScroller: tunnelRenderer не назначен!");
            enabled = false;
            return;
        }
        // .material создаёт копию, чтобы не менять общий материал у всех
        mat = tunnelRenderer.material;
    }

    void Update()
    {
        if (player == null || mat == null) return;

        float playerSpeed = player.CurrentVelZ; // 0 когда стоит, +вперёд, -назад
        scrollOffset += playerSpeed * factor * Time.deltaTime;

        mat.SetFloat("_ScrollOffset", scrollOffset);
    }
}