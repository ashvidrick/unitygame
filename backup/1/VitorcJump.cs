using UnityEngine;

[RequireComponent(typeof(VitorcMotor))]
public class VitorcJump : MonoBehaviour
{
    [Header("Прыжок")]
    public float jumpDuration = 0.55f;   // сколько игрок «висит» на верхнем уровне
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Присед")]
    public float crouchDuration = 0.55f; // сколько игрок сидит внизу
    public KeyCode crouchKey = KeyCode.LeftShift;

    // private VitorcMotor motor;
    public VitorcMotor motor;

    private int baseLevel;         // уровень, на который вернёмся
    private bool inAir;             // сейчас в прыжке
    private bool inCrouch;          // сейчас в приседе
    private float stateTimer;

    void Awake()
    {
        //motor = GetComponent<VitorcMotor>();
        baseLevel = motor.currentLevel;
    }

    void Update()
    {
        // Прыжок
        if (Input.GetKeyDown(jumpKey) && !inAir && !inCrouch)
        {
            baseLevel = motor.currentLevel;                  // запоминаем, откуда прыгнули
            if (motor.TrySetLevel(motor.currentLevel + 1))   // поднимаемся на 1 уровень
            {
                inAir = true;
                stateTimer = jumpDuration;
            }
        }

        // Присед
        if (Input.GetKeyDown(crouchKey) && !inAir && !inCrouch)
        {
            baseLevel = motor.currentLevel;
            if (motor.TrySetLevel(motor.currentLevel - 1))
            {
                inCrouch = true;
                stateTimer = crouchDuration;
            }
        }

        // Таймеры состояния
        if (inAir)
        {
            stateTimer -= Time.deltaTime;
            if (stateTimer <= 0f)
            {
                motor.TrySetLevel(baseLevel);   // возвращаемся на исходный уровень
                inAir = false;
            }
        }
        else if (inCrouch)
        {
            stateTimer -= Time.deltaTime;
            if (stateTimer <= 0f)
            {
                motor.TrySetLevel(baseLevel);
                inCrouch = false;
            }
        }
    }

    // Публичные флаги — пригодятся для анимаций и коллизий
    public bool IsInAir => inAir;
    public bool IsInCrouch => inCrouch;
}