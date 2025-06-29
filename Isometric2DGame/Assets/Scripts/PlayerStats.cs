using UnityEngine;

//summary>
// Handles the player's core stats, including health, movement speed, and combat abilities.
// </summary>
public class PlayerStats : MonoBehaviour
{
    [Header("Core Stats")]
    [Tooltip("Maximum health of the player")]
    [SerializeField] private int maxHealth = 100;
    [Tooltip("Movement speed of the player")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("Combat Stats")]
    
    [Header("Melee Combat")]
    [Tooltip("Melee Damage dealt by the player")]
    [SerializeField] private int mDamage = 50;
    [Tooltip("Cooldown time in seconds between melee attacks")]
    [SerializeField] private float mAttackCooldown = 1f;
    [Tooltip("Range of the player's melee attack")]
    [SerializeField] private float mAttackRange = 1.5f;
    
    [Header("Ranged Combat")]
    [Tooltip("Range of the player's ranged attack")]
    [SerializeField] private float rAttackRange = 10f;
    [Tooltip("Cooldown time in seconds between ranged attacks")]
    [SerializeField] private float rAttackCooldown = 0.5f;
    [Tooltip("Damage dealt by the player's ranged attack")]
    [SerializeField] private int rDamage = 20;

    public int MaxHealth => maxHealth;
    public float MoveSpeed => moveSpeed;
    public int MDamage => mDamage;
    public float MAttackCooldown => mAttackCooldown;
    public float MAttackRange => mAttackRange;
    public float RAttackRange => rAttackRange;
    public float RAttackCooldown => rAttackCooldown;
    public int RDamage => rDamage;

}
