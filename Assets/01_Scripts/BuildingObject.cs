using UnityEngine;

public class BuildingObject : MonoBehaviour
{
    public float hp = 50f; // 건축물 체력

    public void GetDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0) Destroy(gameObject); // 체력이 0이 되면 파괴
    }
}