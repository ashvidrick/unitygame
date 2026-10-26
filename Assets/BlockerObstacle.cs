/*using UnityEngine;

public class BlockerObstacle : MonoBehaviour
{
    [Header("Эффект при ударе")]
    public AudioClip hitSound;
    public float cameraShake = 0.2f;

    [Header("Отладка")]
    public bool showDebug = false;

    void OnCollisionEnter(Collision collision)
    {
        var motor = collision.gameObject.GetComponentInParent<VitorcMotor>();
        if (motor == null) return;

        if (showDebug) Debug.Log("Blocker HIT — player stopped");

        // звук
        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, transform.position);

        // тряска камеры — если есть скрипт
        // CameraShake.Shake(cameraShake);
    }
}
*/
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BlockerObstacle : MonoBehaviour
{
    [Header("Отскок")]
    [Tooltip("Сила отбрасывания назад при ударе (единиц в секунду)")]
    public float bounceBackForce = 8f;

    [Tooltip("Длительность отбрасывания (сек)")]
    public float bounceDuration = 0.15f;

    [Header("Эффекты")]
    public AudioClip hitSound;

    [Tooltip("Куда отбрасывать? 0 = по Z, 1 = от центра блокера")]
    [Range(0f, 1f)] public float pushDirectionMode = 0f;

    [Header("Отладка")]
    public bool showDebug = false;

    private BoxCollider col;
    private bool isBouncing = false;

    void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // вызывается, когда CharacterController игрока врезается в этот коллайдер
        ///if (hit.collider != col) return;
        if (isBouncing) return;

        var motor = hit.controller.GetComponent<VitorcMotor>();
        if (motor == null) return;

        // вычисляем направление отскока
        Vector3 bounceDir;
        if (pushDirectionMode < 0.5f)
        {
            // отскок назад по Z
            bounceDir = Vector3.back;
        }
        else
        {
            // отскок от центра блокера
            bounceDir = (hit.controller.transform.position - transform.position).normalized;
            bounceDir.y = 0f;
        }

        StartCoroutine(DoBounce(motor, bounceDir));

        // звук
        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, hit.point);
    }

    System.Collections.IEnumerator DoBounce(VitorcMotor motor, Vector3 dir)
    {
        isBouncing = true;

        if (showDebug) Debug.Log($"Blocker HIT — bounce {dir}");

        // применяем отбрасывание
        motor.ApplyBounce(dir * bounceBackForce);

        yield return new WaitForSeconds(bounceDuration);

        isBouncing = false;
    }
    void OnCol2lisionEnter(Collision collision)
    {
        var motor = collision.gameObject.GetComponentInParent<VitorcMotor>();
        if (motor == null) return;

        if (showDebug) Debug.Log("Blocker HIT — player stopped");

        // звук
        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, transform.position);

        // тряска камеры — если есть скрипт
        // CameraShake.Shake(cameraShake);
    }

}