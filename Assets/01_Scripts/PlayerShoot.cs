using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    public Camera mainCam;                   // 메인 카메라
    public CameraFollow cameraFollow;        // 메인 카메라의 CameraFollow 스크립트

    public Weapon currentWeapon;

    public float precisionDistance = 2.4f;   // 정밀 사격 카메라 거리
    public float precisionHeight = 0.4f;
    public float precisionShoulderOffset = 0.65f;

    // 원래 카메라 위치 기록
    private float normalDistance;
    private float normalHeight;
    private float normalShoulderOffset;

    // 사격 관련
    private float nextFireTime;
    private int burstShotsRemaining;
    private bool switchModeAfterBurst;

    public bool isPrecisionMode;             // 정밀 사격 상태
    public BuildingSystem buildingSystem;

    void Start()
    {
        mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogError("Not Find Main Cam");
            enabled = false;
            return;
        }
        if (currentWeapon == null)
        {
            Debug.LogError("Not Find Current Weapon");
            enabled = false;
            return;
        }

        cameraFollow = mainCam.GetComponent<CameraFollow>();
        if (cameraFollow == null)
        {
            Debug.LogError("Not Find CameraFollow");
            enabled = false;
            return;
        }

        normalDistance = cameraFollow.distance;
        normalHeight = cameraFollow.height;
        normalShoulderOffset = cameraFollow.shoulderOffset;
    }

    void Update()
    {
        if (buildingSystem != null &&
        (buildingSystem.isBuildMode || buildingSystem.isRemoveMode))
        {
            return;
        }

        UpdatePrecisionMode();
        HandleFireInput();
        HandleFireModeSwitch();
    }

    void HandleFireModeSwitch()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (currentWeapon.curFireMode == Weapon.FireMode.Burst && burstShotsRemaining > 0)
            {
                switchModeAfterBurst = true;
            }
            else
            {
                currentWeapon.SwitchFireMode();
            }
        }
    }

    void UpdatePrecisionMode()
    {
        isPrecisionMode = Input.GetMouseButton(1);

        if (isPrecisionMode)
        {
            SetCamera(precisionDistance, precisionHeight, precisionShoulderOffset);
        }
        else
        {
            SetCamera(normalDistance, normalHeight, normalShoulderOffset);
        }
    }

    void SetCamera(float distance, float height, float shoulderOffset)
    {
        cameraFollow.distance = distance;
        cameraFollow.height = height;
        cameraFollow.shoulderOffset = shoulderOffset;
    }

    void HandleFireInput()
    {
        switch (currentWeapon.curFireMode)
        {
            case Weapon.FireMode.Single:
                HandleSingleFire();
                break;
            case Weapon.FireMode.Auto:
                HandleAutoFire();
                break;
            case Weapon.FireMode.Burst:
                HandleBurstFire();
                break;
        }
    }

    void HandleSingleFire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryFireOneShot();
        }
    }

    void HandleAutoFire()
    {
        if (Input.GetMouseButton(0))
        {
            TryFireOneShot();
        }
    }

    void HandleBurstFire()
    {
        if (Input.GetMouseButtonDown(0) && burstShotsRemaining == 0)
        {
            burstShotsRemaining = currentWeapon.burstCount;
        }

        if (burstShotsRemaining > 0 && TryFireOneShot())
        {
            burstShotsRemaining--;

            if (burstShotsRemaining == 0 && switchModeAfterBurst)
            {
                currentWeapon.SwitchFireMode();
                switchModeAfterBurst = false;
            }
        }
    }

    // 데미지 로직이 포함된 Shoot 함수
    void Shoot(Vector3 viewportPoint)
    {
        Ray ray = mainCam.ViewportPointToRay(viewportPoint);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, currentWeapon.shootDistance))
        {
            Debug.Log("Hit: " + hit.collider.name);

            EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentWeapon.damage);
            }
        }
    }

    bool TryFireOneShot()
    {
        if (Time.time < nextFireTime)
        {
            return false;
        }

        FireOneShot();
        nextFireTime = Time.time + currentWeapon.fireInterval;
        return true;
    }

    void FireOneShot()
    {
        float spread;
        if (isPrecisionMode)
        {
            spread = currentWeapon.precisionSpread;
        }
        else
        {
            spread = currentWeapon.hipFireSpread;
        }

        float randomX = Random.Range(0.5f - spread, 0.5f + spread);
        float randomY = Random.Range(0.5f - spread, 0.5f + spread);

        Shoot(new Vector3(randomX, randomY, 0f));
    }
}