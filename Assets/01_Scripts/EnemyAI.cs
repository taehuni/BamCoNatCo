using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("적군 설정")]
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
        // 1. 공격 범위 내에 BuildingObject가 있는지 탐지
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

        // 2. 건축물이 있으면 공격
        if (targetBuilding != null)
        {
            if (agent.enabled) agent.isStopped = true;
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                targetBuilding.GetDamage(attackDamage);
                lastAttackTime = Time.time;
            }
        }
        // 3. 없으면 코어를 향해 이동하거나 공격
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

    public void TakeDamage(int defaultDamage)
    {
        health -= defaultDamage;
        if (health <= 0) Destroy(gameObject);
    }
}