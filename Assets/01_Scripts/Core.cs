using UnityEngine;

public class Core : MonoBehaviour
{
    public float hp = 100f; // 기본 체력 설정
    public float defensePower = 0f;

    void Update()
    {
        GameOver();
    }

    // 외부(적군)에서 호출할 데미지 처리 함수
    public void GetDamage(float damage)
    {
        // 방어력을 고려한 데미지 계산 (방어력이 데미지를 상쇄)
        float finalDamage = Mathf.Max(0, damage - defensePower);
        hp -= finalDamage;
        Debug.Log($"Core가 {finalDamage}의 데미지를 입었습니다! 남은 체력: {hp}");
    }

    void GameOver()
    {
        if(hp <= 0)
        {
            Debug.Log("Game Over");
        }
    }
}