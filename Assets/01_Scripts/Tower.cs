using UnityEngine;

public class Tower : MonoBehaviour
{
    public float attackRange = 10f;
    public float attackInterval = 1f;
    public int damage = 10;
    public LayerMask enemyLayer;

    private float nextAttackTime;

    void Start()
    {

    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            Attack();

            nextAttackTime = Time.time + attackInterval;
        }
    }

    void Attack()
    {
        //원형 범위에 enemy layer 있는 놈이 찾아
        Collider[] enemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        //없으면 동작이 안해
        if (enemies.Length == 0)
        {
            return;
        }
        //첫번째 찾는 놈
        Collider target = enemies[0];

        EnemyAI enemy = target.GetComponentInParent<EnemyAI>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

    }

}
