using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Àû±º ¼³Á¤")]
    public int health = 30;
    public float moveSpeed = 3f;
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    private NavMeshAgent agent;
    private Core core;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.speed = moveSpeed;
        core = FindObjectOfType<Core>();
    }

    void Update()
    {
        // 1. °ø°Ý ¹üÀ§ ³»¿¡ BuildingObject°¡ ÀÖ´ÂÁö Å½Áö
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        BuildingObject targetBuilding = null;

        foreach (var hit in hitColliders)
        {
            BuildingObject building = hit.GetComponentInParent<BuildingObject>();
            if (building != null)
            {
                targetBuilding = building;
                break;
            }
        }

        // 2. °ÇÃà¹°ÀÌ ÀÖÀ¸¸é °ø°Ý
        if (targetBuilding != null)
        {
            if (agent.enabled) agent.isStopped = true;
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                targetBuilding.GetDamage(attackDamage);
                lastAttackTime = Time.time;
            }
        }
        // 3. ¾øÀ¸¸é ÄÚ¾î¸¦ ÇâÇØ ÀÌµ¿ÇÏ°Å³ª °ø°Ý
        else if (core != null)
        {
            if (agent.enabled) agent.isStopped = false;
            float distance = Vector3.Distance(transform.position, core.transform.position);

            if (distance <= attackRange)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    core.GetDamage(attackDamage);
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                agent.SetDestination(core.transform.position);
            }
        }
    }

<<<<<<< HEAD
    // 1. ï¿½Ìµï¿½ ï¿½Ô¼ï¿½
    public void Move(Vector3 targetPosition)
    {
        if (agent != null)
        {
            agent.SetDestination(targetPosition);
        }
    }

    // 2. ï¿½ï¿½ï¿½ï¿½ ï¿½Ô¼ï¿½
    public void Attack()
    {
        // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ (ï¿½ï¿½: ï¿½Ö´Ï¸ï¿½ï¿½Ì¼ï¿½ ï¿½ï¿½ï¿½ ï¿½ï¿½)
        Debug.Log("ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Õ´Ï´ï¿½!");
    }

    // 3. ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Ã³ï¿½ï¿½ ï¿½Ô¼ï¿½
    public void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage :" + damage);
        health -= damage;
        if (health <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("ï¿½ï¿½ï¿½ï¿½ ï¿½×¾ï¿½ï¿½ï¿½ï¿½Ï´ï¿½!");
        Destroy(gameObject);
=======
    public void TakeDamage(int defaultDamage)
    {
        health -= defaultDamage;
        if (health <= 0) Destroy(gameObject);
>>>>>>> master
    }
}