using JetBrains.Annotations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed; //이동속도
    public int playHp; //플레이어 체력
    public int playerMaxHp; //플레이어 최대 체력(초과불가)
    public Camera mainCam; //메인 카메라
    public float rotateSpeed; //회전 속도

    private CharacterController controller;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();//자기의 CharacterController 가져와
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }

    public void PlayerMove() //플레이어 이동함수
    {
        float horizontal = Input.GetAxis("Horizontal"); //좌우 ad
        float vertical = Input.GetAxis("Vertical"); //상하 ws
        Vector3 moveDir = new Vector3(horizontal, 0, vertical).normalized; //방향값 가져와
        if (moveDir.magnitude > 1f)
        {
            moveDir.Normalize(); //방향값 1초과이면 1로 초기화(wa 같은 2키누릴때 같은 속도)
        }

        Vector3 velocity = moveDir * moveSpeed; //moveSpeed
        controller.Move(velocity * Time.deltaTime); //이동
    }

}
