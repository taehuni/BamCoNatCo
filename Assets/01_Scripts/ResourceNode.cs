using UnityEngine;
using UnityEngine.UI;

public class ResourceNode : MonoBehaviour
{
    public float detectRange = 2f;
    public LayerMask playerLayer;

    public float collectTime = 3f;

    private bool playerInRange;
    private bool isCollecting;
    private float collectTimer;
    private PlayerInteractUI playerUI;
    private Slider playerSlider;


    void Update()
    {
        CheckPlayerNear();

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartCollect();
        }

        if (isCollecting)
        {
            UpdateCollect();
        }
    }

    void CheckPlayerNear()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, detectRange, playerLayer);

        playerInRange = players.Length > 0;


        if (playerInRange)
        {
            playerUI = players[0].GetComponentInParent<PlayerInteractUI>();
            if (playerUI != null)
            {
                playerSlider = playerUI.interactSlider;
                playerUI.ShowButton("채집하기(E)");
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

        if (!playerInRange)
        {
            StopCollect();
        }
    }

    void StartCollect()
    {
        isCollecting = true;
        collectTimer = 0f;

        if (playerUI != null)
        {
            playerUI.ShowSlider();
        }

        if (playerSlider != null)
        {
            playerSlider.value = 0f;
        }
    }

    void UpdateCollect()
    {
        collectTimer += Time.deltaTime;

        if (playerSlider != null)
        {
            playerSlider.value = collectTimer / collectTime;
        }

        if (collectTimer >= collectTime)
        {
            CollectComplete();
        }
    }

    void CollectComplete()
    {
        isCollecting = false;
        collectTimer = 0f;

        if (playerSlider != null)
        {
            playerSlider.value = 0f;
        }

        if (playerUI != null)
        {
            playerUI.HideSlider();
        }

        Debug.Log("Collect Complete");
    }

    void StopCollect()
    {
        isCollecting = false;
        collectTimer = 0f;

        if (playerSlider != null)
        {
            playerSlider.value = 0f;
        }

        if (playerUI != null)
        {
            playerUI.HideSlider();
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}