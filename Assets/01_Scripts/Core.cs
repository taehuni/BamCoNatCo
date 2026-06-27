using UnityEngine;

public class Core : MonoBehaviour
{
    public float hp;
    public float defensePower;

    void Update()
    {
        GameOver();
    }

    //다매지 받기
    void GetDamage(float damage)
    {
        
    }

    //hp < 0 미면 게임 종료
    void GameOver()
    {
        if(hp <= 0)
        {
            Debug.Log("Game Over");
        }
    }
}
