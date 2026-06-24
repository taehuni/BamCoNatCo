using UnityEngine;
using System.Collections;
public class PlayerShoot : MonoBehaviour
{
    public Camera mainCam;                 // 主相机 메인 카메라
    public CameraFollow cameraFollow;      // 主相机上的 CameraFollow 脚本 메인카메라 달려있는 CameraFollow Script

    public Weapon currentWeapon;

    public float precisionDistance = 2.4f;   // 按右键后相机距离 정밀 사격 카메라 거리
    public float precisionHeight = 0.4f;
    public float precisionShoulderOffset = 0.65f;

    // 记录原本相机距离 원래 카메라 위치 기록
    private float normalDistance;
    private float normalHeight;
    private float normalShoulderOffset;
    //사격 관한
    private float nextFireTime; //사격 간격
    private int burstShotsRemaining; //burst 모드의 사격 수
    private bool switchModeAfterBurst; //burst 도중 모드 변환 체크

    public bool isPrecisionMode;          // 当前是否处于精准射击状态 정밀 사격 상태

    void Start()
    {
        mainCam = Camera.main;
        //메인 카메라 없으면
        if (mainCam == null)
        {
            Debug.LogError("Not Find Main Cam");
            enabled = false;
            return;
        }
        //무기 없으면
        if (currentWeapon == null)
        {
            Debug.LogError("Not Find Current Weapon");
            enabled = false;
            return;
        }

        cameraFollow = mainCam.GetComponent<CameraFollow>();
        //cameraFollow script 없으면
        if (cameraFollow == null)
        {
            Debug.LogError("Not Find CameraFollow");
            enabled = false;
            return;
        }

        //원래의 카메라 위치 저장
        normalDistance = cameraFollow.distance;
        normalHeight = cameraFollow.height;
        normalShoulderOffset = cameraFollow.shoulderOffset;
    }

    void Update()
    {

        UpdatePrecisionMode();//정밀 사격
        HandleFireInput();//사격 모드
        HandleFireModeSwitch();//사격 모드 전환
    }

    void HandleFireModeSwitch()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            // 当前正在三连发，先记录切换请求
            //burst 상태이고 남은 총알도 있는 상태이면
            if (currentWeapon.curFireMode == Weapon.FireMode.Burst &&
                burstShotsRemaining > 0)
            {
                switchModeAfterBurst = true;
            }
            else
            {
                // 不在三连发中，可以立即切换
                //위에 그 상태 아니면 금방 전환
                currentWeapon.SwitchFireMode();
            }
        }
    }

    // 按住右键进入精准射击状态
    //마우스 오른쪽 버튼 클릭 정밀 사격상태 들어가
    void UpdatePrecisionMode()
    {
        isPrecisionMode = Input.GetMouseButton(1);

        if (isPrecisionMode)
        {
            SetCamera(
                precisionDistance,
                precisionHeight,
                precisionShoulderOffset
            );
        }
        else
        {
            SetCamera(
                normalDistance,
                normalHeight,
                normalShoulderOffset
            );
        }
    }

    //카메라 위치 조정(사격모드에 따라)
    void SetCamera(float distance, float height, float shoulderOffset)
    {
        cameraFollow.distance = distance;
        cameraFollow.height = height;
        cameraFollow.shoulderOffset = shoulderOffset;
    }

    //사격 모드 판단
    void HandleFireInput()
    {
        switch (currentWeapon.curFireMode)
        {
            case Weapon.FireMode.Single: //Single 모드
                HandleSingleFire();
                break;

            case Weapon.FireMode.Auto: //Auto 모드
                HandleAutoFire();
                break;

            case Weapon.FireMode.Burst: //Burst 모드
                HandleBurstFire();
                break;
        }
    }

    //Single 모드
    void HandleSingleFire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryFireOneShot();
        }
    }

    //Auto 모드
    void HandleAutoFire()
    {
        if (Input.GetMouseButton(0))
        {
            TryFireOneShot();
        }
    }

    //Burst 모드
    void HandleBurstFire()
    {
        //사격 준비
        if (Input.GetMouseButtonDown(0) && burstShotsRemaining == 0)
        {
            burstShotsRemaining = currentWeapon.burstCount;
        }

        //발사 + 한번에 남은 총알 -1
        if (burstShotsRemaining > 0 && TryFireOneShot())
        {
            burstShotsRemaining--;

            //만약 발사하는 도중 모드 전환하면 남은 총알 다 발사할 때 까지 모드 전환
            if (burstShotsRemaining == 0 && switchModeAfterBurst)
            {
                currentWeapon.SwitchFireMode();

                switchModeAfterBurst = false;
            }
        }
    }


    void Shoot(Vector3 viewportPoint)
    {
        //위치를 가져왔어 그위치에서 사선 발사
        Ray ray = mainCam.ViewportPointToRay(viewportPoint);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, currentWeapon.shootDistance))
        {
            Debug.Log("Hit: " + hit.collider.tag);
        }
    }


    //사격 시도(사격 간격 지나지 않으면 다시 사격 불가)
    bool TryFireOneShot()
    {
        //사격 간격 지나지 않으면 사격 안 해
        if (Time.time < nextFireTime)
        {
            return false;
        }

        //발사
        FireOneShot();

        //다음 사격 가능 한 시간 계산
        nextFireTime = Time.time + currentWeapon.fireInterval;

        //사격 성공
        return true;
    }

    void FireOneShot()
    {
        float spread; //Temp 값

        //정밀사격 판단
        if (isPrecisionMode)
        {
            spread = currentWeapon.precisionSpread;
        }
        else
        {
            spread = currentWeapon.hipFireSpread;
        }

        //사격 위치 계산(사격모드에 따로)
        float randomX = Random.Range(0.5f - spread, 0.5f + spread);
        float randomY = Random.Range(0.5f - spread, 0.5f + spread);

        //사선 발사
        Shoot(new Vector3(randomX, randomY, 0f));
    }
}