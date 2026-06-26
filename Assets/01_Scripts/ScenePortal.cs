using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    public float detectRange = 2f;
    public LayerMask playerLayer;
    public GameObject 传送按钮;
    public string targetSceneName;

    private bool playerInRange;

    void Start()
    {
        if (传送按钮 != null)
        {
            传送按钮.SetActive(false);
        }
    }

    void Update()
    {
        玩家靠近检测();

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            LoadTargetScene();
        }
    }

    void 玩家靠近检测()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, detectRange, playerLayer);

        playerInRange = players.Length > 0;

        if (传送按钮 != null)
        {
            传送按钮.SetActive(playerInRange);
        }
    }

    void LoadTargetScene()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("Target scene name is empty.");
            return;
        }

        SceneManager.LoadScene(targetSceneName);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}