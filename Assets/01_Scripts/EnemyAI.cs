using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("적군 설정")]
    public int health = 30;
    public float moveSpeed = 3f;
    public float attackRange = 2f;
    public float attackDamage = 10f; // 공격력
    public float attackCooldown = 1.5f; // 공격 간격
    private float lastAttackTime;

    [Header("참조")]
    public Core core; // 플레이어 대신 Core 참조

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.speed = moveSpeed;

        // 씬에 있는 Core를 자동으로 찾고 싶다면 아래 주석을 해제하세요.
        if (core == null) core = FindObjectOfType<Core>();
    }

    void Update()
    {
        if (core == null) return;

        float distance = Vector3.Distance(transform.position, core.transform.position);

        if (distance <= attackRange)
        {
            // 공격 범위 안에 들어오면 공격 시도
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            // Core를 향해 이동
            Move(core.transform.position);
        }
    }

    public void Move(Vector3 targetPosition)
    {
        if (agent != null && agent.enabled)
        {
            agent.SetDestination(targetPosition);
        }
    }

    public void Attack()
    {
        Debug.Log("Core 공격!");
        // Core의 GetDamage 함수를 호출하여 데미지 전달
        core.GetDamage(attackDamage);
    }

    // 데미지 처리 함수
    public void TakeDamage(int defaultDamage)
    {
        health -= defaultDamage;
        Debug.Log($"받은 데미지: {defaultDamage}, 남은 체력: {health}");

        if (health <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("적군이 사망했습니다!");
        Destroy(gameObject);
    }
}