using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float maxRange;
    private int damage;
    private Vector2 startPosition;

    public void Initialize(Vector2 direction, float speed, float maxRange, int damage)
    {
        this.direction = direction.normalized;
        this.speed = speed;
        this.maxRange = maxRange;
        this.damage = damage;
        this.startPosition = transform.position;
    }

    private void Update()
    {
        // Move the projectile
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // Destroy if max range reached
        if (Vector2.Distance(startPosition, transform.position) >= maxRange)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for enemy hit (tag or component)
        var enemy = collision.GetComponent<EnemyStats>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        // Optionally: destroy on hitting anything else except the player
        else if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
