using UnityEngine;

public class ObstacleHit : MonoBehaviour
{
    [Header("Эффект")]
    public bool destroyOnHit = true;
    public GameObject hitFx;

    void OnTriggerEnter(Collider other)
    {
        var motor = other.GetComponentInParent<VitorcMotor>();
        if (motor == null) return;

        Debug.Log("HIT");

        if (hitFx) Instantiate(hitFx, transform.position, Quaternion.identity);
        if (destroyOnHit) Destroy(gameObject);

        // motor.GetComponent<PlayerHealth>()?.Damage(1);
    }
}