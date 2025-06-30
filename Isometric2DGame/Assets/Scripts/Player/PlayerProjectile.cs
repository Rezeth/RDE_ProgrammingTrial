using UnityEngine;

/// <summary>
/// Handles the behavior of a projectile fired by the player, including movement, collision, and damage.
/// </summary>
public class PlayerProjectile : MonoBehaviour
{
    // The normalized direction in which the projectile travels
    private Vector2 direction;
    // The speed at which the projectile moves
    private float speed;
    // The maximum distance the projectile can travel before being destroyed
    private float maxRange;
    // The amount of damage this projectile deals to enemies
    private int damage;
    // The position where the projectile was spawned
    private Vector2 startPosition;

    /// <summary>
    /// Initializes the projectile's movement and damage parameters.
    /// </summary>
    /// <param name="direction">Direction to move in (will be normalized).</param>
    /// <param name="speed">Movement speed of the projectile.</param>
    /// <param name="maxRange">Maximum distance the projectile can travel.</param>
    /// <param name="damage">Damage dealt to enemies on hit.</param>
    public void Initialize(Vector2 direction, float speed, float maxRange, int damage)
    {
        this.direction = direction.normalized;
        this.speed = speed;
        this.maxRange = maxRange;
        this.damage = damage;
        this.startPosition = transform.position;
    }

    /// <summary>
    /// Moves the projectile each frame and destroys it if it exceeds its maximum range.
    /// </summary>
    private void Update()
    {
        // Move the projectile in the set direction
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // Destroy the projectile if it has traveled beyond its maximum range
        if (Vector2.Distance(startPosition, transform.position) >= maxRange)
            Destroy(gameObject);
    }

    /// <summary>
    /// Handles collision with other objects.
    /// Damages enemies and destroys the projectile on impact.
    /// Ignores collisions with the player.
    /// </summary>
    /// <param name="collision">Collider the projectile has entered.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the projectile hit an enemy
        var enemy = collision.GetComponent<EnemyStats>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        // Destroy the projectile on hitting anything except the player
        else if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
