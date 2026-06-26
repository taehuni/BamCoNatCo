using UnityEngine;

public class Wall : MonoBehaviour
{
    public float hp;
    public float maxHp;
    public int defensePower;
    //public Animator wallAnim;

    public void GetDamage(float damage)
    {
        float finalDamage = damage - defensePower;

        if (finalDamage < 5)
        {
            finalDamage = 5;
        }

        hp -= finalDamage;

        if (hp <= 0)
        {
            WallDestroy();
        }
    }

    void WallDestroy()
    {
        //파괴한 animation
        Destroy(gameObject);
    }
}
