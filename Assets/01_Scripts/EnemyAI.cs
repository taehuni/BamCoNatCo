using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("상태 설정")]
    public int health = 30;
    public float moveSpeed = 3f;
    public float attackRange = 2f;

    [Header("참조")]
    private Weapon weapon; // 공격할 때 무기의 데미지 값을 가져오기 위해 참조
    public Transform player; // 추적할 플레이어

    private UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.speed = moveSpeed;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            Attack();
        }
        else
        {
            Move(player.position);
        }
    }

    // 1. 이동 함수
    public void Move(Vector3 targetPosition)
    {
        if (agent != null)
        {
            agent.SetDestination(targetPosition);
        }
    }

    // 2. 공격 함수
    public void Attack()
    {
        // 공격 로직 (예: 애니메이션 재생 등)
        Debug.Log("적이 공격합니다!");
    }

    // 3. 데미지 처리 함수
    public void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage :" + damage);
        health -= damage;
        if (health <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("적이 죽었습니다!");
        Destroy(gameObject);
    }
}