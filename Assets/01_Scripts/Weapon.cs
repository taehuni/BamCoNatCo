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

    public void SwitchFireMode()
    {
        // 只有一种模式时，不需要切换 //한 모드 만 있으면 변경하지 않아
        if (availableFireModes.Length <= 1)
        {
            return;
        }

        // 当前数组位置加 1 //모드 리스트에 index + 1
        currentModeIndex++;

        // 到达数组末尾时，回到第一个模式 //모드 리스트 초과하면 첫 모드에 다시 돌어가
        if (currentModeIndex >= availableFireModes.Length)
        {
            currentModeIndex = 0;
        }

        // 用新位置更新当前开火模式 //현재의 사격 모드 변환
        curFireMode = availableFireModes[currentModeIndex];

        Debug.Log("Now Fire Mode: " + curFireMode);
    }

}
