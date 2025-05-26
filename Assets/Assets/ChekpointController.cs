using UnityEngine;

public class ChekpointController : MonoBehaviour
{
    public PlayerController playerController;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.updateCheckpoint();
        }
    }
}
