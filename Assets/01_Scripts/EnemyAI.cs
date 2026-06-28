using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("���� ����")]
    public int health = 30;
    public float moveSpeed = 3f;
    public float attackRange = 2f;

    [Header("����")]
    private Weapon weapon; // ������ �� ������ ������ ���� �������� ���� ����
    public Transform player; // ������ �÷��̾�

    private UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.speed = moveSpeed;
        weapon = player.GetComponentInChildren<Weapon>();
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

    // 1. �̵� �Լ�
    public void Move(Vector3 targetPosition)
    {
        if (agent != null)
        {
            agent.SetDestination(targetPosition);
        }
    }

    // 2. ���� �Լ�
    public void Attack()
    {
        // ���� ���� (��: �ִϸ��̼� ��� ��)
        Debug.Log("���� �����մϴ�!");
    }

    // 3. ������ ó�� �Լ�
    public void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage :" + damage);
        health -= damage;
        if (health <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("���� �׾����ϴ�!");
        Destroy(gameObject);
    }
}