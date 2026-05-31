using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [Header("Zombie Stats")]
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackRange = 1.8f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float health = 30f;
    [SerializeField] private int coinReward = 5;

    private Transform player;
    private NavMeshAgent agent;

    private float lastAttackTime;

    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if (p != null)
        {
            player = p.transform;

            Debug.Log(name + " menemukan player!");
        }
        else
        {
            Debug.LogWarning("Player tidak ditemukan!");
        }
    }

    void Update()
    {
        if (isDead || player == null)
            return;

        float dist = Vector3.Distance(
            transform.position,
            player.position
        );

        // Kejar player
        if (dist <= detectionRange)
        {
            agent.SetDestination(player.position);

            // Serang jika dekat
            if (dist <= attackRange)
            {
                TryAttack();
            }
        }
        else
        {
            agent.ResetPath();
        }
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        PlayerHealth ph =
            player.GetComponent<PlayerHealth>();

        if (ph != null)
        {
            ph.TakeDamage(attackDamage);

            Debug.Log(
                name +
                " menyerang player! Damage: "
                + attackDamage
            );
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        health -= damage;

        Debug.Log(
            name +
            " terkena damage! HP zombie: "
            + health
        );

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        Debug.Log(
            name +
            " berhasil dibunuh!"
        );

        GameManager.Instance?.OnZombieKilled(coinReward);

        Destroy(gameObject);
    }
}