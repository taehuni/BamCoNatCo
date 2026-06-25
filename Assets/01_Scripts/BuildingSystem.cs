using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public Camera mainCam;
    public GameObject wallPrefab;
    public GameObject towerPrefab;
    private GameObject currentBuildingPrefab;
    private GameObject previewBuilding;
    public float buildDistance = 10f;

    public LayerMask buildRayLayer;
    public bool isBuildMode;
    public float gridSize = 1f;
    public float checkBoxScale = 0.95f;
    public Vector3 checkBoxHalfSize = new Vector3(0.45f, 0.45f, 0.45f);
    public LayerMask blockedLayer;


    private bool canBuild;
    public Material canBuildMaterial;
    public Material cannotBuildMaterial;
    private Vector3 currentBuildPosition;
    public float rotateAngle = 90f;
    private float currentRotationY;

    void Start()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
        currentBuildingPrefab = wallPrefab;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBuildMode = !isBuildMode;

            if (isBuildMode)
            {
                CreatePreview();
            }
            else
            {
                DestroyPreview();
                currentRotationY = 0f;
            }
        }

        if (isBuildMode)
        {
            HandleBuildingSelection();
            UpdatePreview();

            if (Input.GetKeyDown(KeyCode.R))
            {
                RotatePreview();
            }

            if (Input.GetMouseButtonDown(0) && canBuild)
            {
                TryBuild();
            }
        }
    }


    //건조 한 대상 선택
    void HandleBuildingSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectBuilding(wallPrefab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectBuilding(towerPrefab);
        }
    }

    //선택함수
    void SelectBuilding(GameObject prefab)
    {
        if (prefab == null)
        {
            return;
        }

        currentBuildingPrefab = prefab;

        if (isBuildMode)
        {
            DestroyPreview();
            CreatePreview();
        }
    }


    //그리드 계산 함수
    Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float z = Mathf.Round(position.z / gridSize) * gridSize;

        return new Vector3(x, position.y, z);
    }

    //미리보기 재질 변경함수
    void SetPreviewMaterial(Material material)
    {
        if (previewBuilding == null || material == null)
        {
            return;
        }
        Renderer[] renderers = previewBuilding.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = material;
        }
    }

    //미리보기 생성
    void CreatePreview()
    {
        if (currentBuildingPrefab == null)
        {
            return;
        }

        previewBuilding = Instantiate(currentBuildingPrefab);

        Collider[] colliders = previewBuilding.GetComponentsInChildren<Collider>();

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }

    //미리보기 삭제
    void DestroyPreview()
    {
        if (previewBuilding != null)
        {
            Destroy(previewBuilding);
        }
        previewBuilding = null;
    }

    //실시간 미리보기
    void UpdatePreview()
    {
        if (previewBuilding == null)
        {
            return;
        }

        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, buildDistance, buildRayLayer))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Vector3 snappedPosition = SnapToGrid(hit.point);

                currentBuildPosition = snappedPosition + Vector3.up * GetBuildingHeightOffset();

                previewBuilding.SetActive(true);

                previewBuilding.transform.position = currentBuildPosition;
                previewBuilding.transform.rotation = Quaternion.Euler(0f, currentRotationY, 0f);

                canBuild = !IsPositionBlocked();

                UpdatePreviewMaterial();
            }
            else
            {
                canBuild = false;

                previewBuilding.SetActive(false);
            }

        }
        else
        {
            canBuild = false;
            previewBuilding.SetActive(false);

        }

    }

    //미리보기 색깔을 건조가능한 상태에 따라 변경
    void UpdatePreviewMaterial()
    {
        if (canBuild)
        {
            SetPreviewMaterial(canBuildMaterial);
        }
        else
        {
            SetPreviewMaterial(cannotBuildMaterial);
        }
    }

    //간조 시도
    void TryBuild()
    {
        Quaternion buildRotation = Quaternion.Euler(0f, currentRotationY, 0f);
        Instantiate(currentBuildingPrefab, currentBuildPosition, buildRotation);
    }

    //간조 위치 높이 계산함수
    float GetBuildingHeightOffset()
    {
        Renderer renderer = previewBuilding.GetComponentInChildren<Renderer>();

        if (renderer == null)
        {
            return 0f;
        }

        return previewBuilding.transform.position.y - renderer.bounds.min.y;
    }

    //건조 위치에서 충돌 판단
    bool IsPositionBlocked()
    {
        Renderer renderer = previewBuilding.GetComponentInChildren<Renderer>();

        if (renderer == null)
        {
            return Physics.CheckBox(currentBuildPosition, checkBoxHalfSize, Quaternion.identity, blockedLayer);
        }

        Vector3 center = renderer.bounds.center;
        Vector3 halfSize = renderer.bounds.size / 2f * checkBoxScale;

        return Physics.CheckBox(center, halfSize, previewBuilding.transform.rotation, blockedLayer);
    }

    //미리보기 회전
    void RotatePreview()
    {
        currentRotationY += rotateAngle;

        if (previewBuilding != null)
        {
            previewBuilding.transform.rotation = Quaternion.Euler(0f, currentRotationY, 0f);
        }
    }
}