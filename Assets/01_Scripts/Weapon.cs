using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("다매지")]
    public int damage = 10;
    [Header("사격 유효거리")]
    public float shootDistance = 100f;

    [Header("정밀도")]
    public float hipFireSpread = 0.04f;
    public float precisionSpread = 0.01f;

    [Header("Auto 모드 발사 간격")]
    public float fireInterval = 0.1f;
    [Header("burst 모드 발사수")]
    public int burstCount = 3;

    //사격 방식
    public enum FireMode
    {
        Single, // 마우스 클릭하면 1총알 발사单发：每点一次左键射一发
        Auto,   // 마우스 누르면 총알 계속 발사连发：按住左键持续射击
        Burst   // 마우스 클릭하면 3총알 손서때로 발사三连发：点一次左键自动射三发
    }

    [Header("현재사격 모드")]
    public FireMode curFireMode;
    [Header("사격 모드 리스트")]
    public FireMode[] availableFireModes;
    //사격 모드 리스트에 index
    private int currentModeIndex;

    void Start()
    {
        if (availableFireModes == null || availableFireModes.Length == 0)
        {
            Debug.LogError("사격 모드 없습니다.");
            enabled = false;
            return;
        }
        //모드 리스트 에 index
        currentModeIndex = 0;
        curFireMode = availableFireModes[currentModeIndex]; //사격 모드 초기화
    }

    // Weapon.cs 내부
    public void SwitchFireMode()
    {
        switch (curFireMode)
        {
            case FireMode.Single:
                curFireMode = FireMode.Auto;
                Debug.Log("모드 변경: Single -> Auto");
                break;
            case FireMode.Auto:
                curFireMode = FireMode.Burst;
                Debug.Log("모드 변경: Auto -> Burst");
                break;
            case FireMode.Burst:
                curFireMode = FireMode.Single;
                Debug.Log("모드 변경: Burst -> Single");
                break;
        }
    }

}
