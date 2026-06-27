using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    public float detectRange = 2f;
    public LayerMask playerLayer;
    public string targetSceneName;

    private bool playerInRange;
    private PlayerInteractUI playerUI;

    void Start()
    {
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

        if (playerInRange)
        {
            playerUI = players[0].GetComponentInParent<PlayerInteractUI>();

            if (playerUI != null)
            {
                playerUI.ShowButton("전송하기(E)");
            }
        }
        else
        {
            if (playerUI != null)
            {
                playerUI.HideButton();
                playerUI = null;
            }
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