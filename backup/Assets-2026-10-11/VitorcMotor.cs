using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VitorcMotor : MonoBehaviour
{
    [Header("Ссылки")]
    public Transform visualRoot;

    [Header("Движение X (влево-вправо)")]
    public float lateralSpeed = 6f;
    public float lateralBounds = 5f;
    public float lateralAccel = 20f;

    [Header("Движение Z (вперёд-назад)")]
    public float forwardSpeed = 8f;
    public float backwardSpeed = 4f;
    public float forwardAccel = 20f;
    public bool lockForward = false;

    [Header("Движение Y (вверх-вниз)")]
    public float verticalSpeed = 5f;
    public float verticalAccel = 20f;
    public float verticalRange = 2.5f;   // половина от lateralBounds/2 и т.п. — задашь в инспекторе
    public KeyCode upKey = KeyCode.Space;
    public KeyCode downKey = KeyCode.LeftShift;

    [Header("Отладка")]
    public bool showDebug = false;

    // ── внутреннее ──
    private CharacterController cc;
    private float currentVelX;
    private float currentVelZ;
    private float currentVelY;

    // ── публичные свойства ──
    public float CurrentVelZ => currentVelZ;
    public float CurrentVelX => currentVelX;
    public float CurrentVelY => currentVelY;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        if (visualRoot == null) visualRoot = transform;
    }

    void Update()
    {
        HandleLateral();
        HandleForward();
        HandleVertical();

        if (showDebug && Time.frameCount % 30 == 0)
        {
            Debug.Log($"vel: X={currentVelX:F2} Y={currentVelY:F2} Z={currentVelZ:F2} " +
                      $"pos: {transform.position}");
        }
    }

    // ────────── X ──────────
    void HandleLateral()
    {
        float input = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) input -= 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) input += 1f;

        float targetVel = input * lateralSpeed;
        currentVelX = Mathf.MoveTowards(currentVelX, targetVel, lateralAccel * Time.deltaTime);

        Vector3 pos = transform.position;
        float nextX = Mathf.Clamp(pos.x + currentVelX * Time.deltaTime, -lateralBounds, lateralBounds);
        float dx = nextX - pos.x;

        cc.Move(new Vector3(dx, 0f, 0f));
    }

    // ────────── Z ──────────
    void HandleForward()
    {
        if (lockForward)
        {
            currentVelZ = Mathf.MoveTowards(currentVelZ, 0f, forwardAccel * Time.deltaTime);
            return;
        }

        float input = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) input += 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) input -= 1f;

        float targetVel;
        if (input > 0.01f) targetVel = forwardSpeed;
        else if (input < -0.01f) targetVel = -backwardSpeed;
        else targetVel = forwardSpeed;   // авто-бег вперёд

        currentVelZ = Mathf.MoveTowards(currentVelZ, targetVel, forwardAccel * Time.deltaTime);
        cc.Move(new Vector3(0f, 0f, currentVelZ * Time.deltaTime));
    }

    // ────────── Y ──────────
    void HandleVertical()
    {
        float input = 0f;
        if (Input.GetKey(upKey)) input += 1f;
        if (Input.GetKey(downKey)) input -= 1f;

        float targetVel = input * verticalSpeed;
        currentVelY = Mathf.MoveTowards(currentVelY, targetVel, verticalAccel * Time.deltaTime);

        Vector3 pos = transform.position;
        float nextY = Mathf.Clamp(pos.y + currentVelY * Time.deltaTime, -verticalRange, verticalRange);
        float dy = nextY - pos.y;

        cc.Move(new Vector3(0f, dy, 0f));
    }

    public void ResetMotion()
    {
        currentVelX = 0f;
        currentVelY = 0f;
        currentVelZ = 0f;
    }
}