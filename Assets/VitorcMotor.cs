/*using UnityEngine;

public class NewEmptyCSharpScript
{
    
}*/
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(CharacterController))]
public class VitorcMotor : MonoBehaviour
{
    [Header("Ссылки")]
    public Transform playerVitorc;          // сюда вешаем спрайт/меш, чтобы не ломать контроллер

    [Header("Фон (скорость)")]
    public float CurrentVelZ => currentVelZ;

    [Header("Движение X/Z")]
    public float forwardSpeed = 8f;     // базовая скорость вперёд
    public float lateralSpeed = 8f;     // скорость влево-вправо
    public float backwardSpeed = 4f;     // назад медленнее
    public float accel = 20f;    // разгон/торможение
    public float lateralBounds = 10f;     // ограничение по X

    [Header("Уровни Y")]
    public int currentLevel = 0;      // -1 / 0 / +1  (можно расширить)
    public int minLevel = -1;
    public int maxLevel = 1;
    public float levelHeight = 2.0f;   // расстояние между этажами
    public float yTransition = 0.12f;  // время lerp по Y

    [Header("Отладка")]
    public bool lockForward = false;

    [Header("Input")]
    public InputActionReference moveAction; // Перетащи сюда созданный Action

    [Header("Вертикаль Y (если есть)")]
    public float verticalSpeed = 5f;
    public float verticalRange = 2.5f;

  

    //public float CurrentVelZ => currentVelZ;


    // --- внутреннее ---
    private CharacterController cc;
    private float currentVelX;             // для сглаженного X
    private float currentVelZ;             // для сглаженного Z

    private float currentVelY;

    private float targetVisualY;           // куда едет визуал
    private float visualYVelocity;         // SmoothDamp

    public int CurrentLevel => currentLevel;
    public float CurrentX => transform.position.x;
    public float CurrentZ => transform.position.z;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        if (playerVitorc == null) playerVitorc = transform;
        targetVisualY = currentLevel * levelHeight;
        moveAction.action.Enable();
    }
    public void ApplyBounce(Vector3 velocity)
    {
        // разложить скорость по осям
        currentVelX = velocity.x;
        currentVelZ = velocity.z;

        // визуально дёрнуть визуал (опционально)
        // можно добавить тряску visualRoot
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.T))
        {
            transform.position = new Vector3(0f, transform.position.y, transform.position.z + 5f);
        }
        HandleLateral();
        HandleForward();
        HandleVerticalVisual();
        */
        Vector2 input = moveAction != null
           ? moveAction.action.ReadValue<Vector2>()
           : Vector2.zero;

        HandleLateral(input.x);
        HandleForward(input.y);
        // HandleVertical(input.y);  // если нужна вертикаль — раскомментируй


    }

    // ---------- X: влево-вправо ----------
    /*void HandleLateral()
    {
        float input = Input.GetAxisRaw("Horizontal"); // A/D или стрелки
        float targetVel = input * lateralSpeed;
        currentVelX = Mathf.MoveTowards(currentVelX, targetVel, accel * Time.deltaTime);

        // ограничение по X — считаем вручную, чтобы не упираться в стены
        Vector3 pos = transform.position;
        float nextX = Mathf.Clamp(pos.x + currentVelX * Time.deltaTime, -lateralBounds, lateralBounds);
        float dx = nextX - pos.x;

        cc.Move(new Vector3(dx, 0f, 0f));
      
        
    }


    // ---------- Z: вперёд-назад ----------
    void HandleForward()
    {
        if (lockForward) { currentVelZ = Mathf.MoveTowards(currentVelZ, 0f, accel * Time.deltaTime); return; }

        float input = Input.GetAxisRaw("Vertical"); // W/S

        //Debug.Log($"input={input}, velZ={currentVelZ:F2}, posZ={transform.position.z:F3}, deltaTime={Time.deltaTime:F4}");

        float targetVel = 0f;

        if (input > 0.01f) targetVel = forwardSpeed;   // вперёд — базовая
        else if (input < -0.01f) targetVel = -backwardSpeed; // назад — медленнее
        else targetVel = 0f;             // автостоп

        currentVelZ = Mathf.MoveTowards(currentVelZ, targetVel, accel * Time.deltaTime);
        cc.Move(new Vector3(0f, 0f, currentVelZ * Time.deltaTime));
    }
    */
    void HandleLateral(float input)
    {
        float targetVel = input * lateralSpeed;
        currentVelX = Mathf.MoveTowards(currentVelX, targetVel, accel * Time.deltaTime);

        Vector3 pos = transform.position;
        float nextX = Mathf.Clamp(pos.x + currentVelX * Time.deltaTime, -lateralBounds, lateralBounds);
        float dx = nextX - pos.x;

        cc.Move(new Vector3(dx, 0f, 0f));
    }

    void HandleForward(float input)
    {
        if (lockForward)
        {
            currentVelZ = Mathf.MoveTowards(currentVelZ, 0f, accel * Time.deltaTime);
            return;
        }

        float targetVel = 0f;
        if (input > 0.01f) targetVel = forwardSpeed;
        else if (input < -0.01f) targetVel = -backwardSpeed;
        else targetVel = 0f;

        currentVelZ = Mathf.MoveTowards(currentVelZ, targetVel, accel * Time.deltaTime);
        cc.Move(new Vector3(0f, 0f, currentVelZ * Time.deltaTime));
    }

    void HandleVertical(float input)
    {
        float targetVel = input * verticalSpeed;
        currentVelY = Mathf.MoveTowards(currentVelY, targetVel, accel * Time.deltaTime);

        Vector3 pos = transform.position;
        float nextY = Mathf.Clamp(pos.y + currentVelY * Time.deltaTime, -verticalRange, verticalRange);
        float dy = nextY - pos.y;

        cc.Move(new Vector3(0f, dy, 0f));
    }


// ---------- Y: визуальный lerp между уровнями ----------
private void HandleVerticalVisual()
    {
        if(targetVisualY > 4)
        {
            var i = 0;
            i++;
        }
        // логическая высота, к которой стремимся
        targetVisualY = currentLevel * levelHeight;

        float newY = Mathf.SmoothDamp(
            playerVitorc.localPosition.y,
            targetVisualY,
            ref visualYVelocity,
            yTransition
        );

        if (targetVisualY > 4)
        {
            var i = 0;
            i++;
        }

        Vector3 lp = playerVitorc.localPosition;
        lp.y = newY;
        playerVitorc.localPosition = lp;
    }

    // ---------- Публичные методы для PlayerJump ----------
    public bool TrySetLevel(int newLevel)
    {
        newLevel = Mathf.Clamp(newLevel, minLevel, maxLevel);
        if (newLevel == currentLevel) return false;
        currentLevel = newLevel;
        return true;
    }

    public void ResetMotion()
    {
        currentVelX = 0f;
        currentVelZ = 0f;
    }
}
