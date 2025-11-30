using UnityEngine;
using UnityEngine.Video;

public class Door : MonoBehaviour
{
    public string KeyName;

// Here, we destroy the doors when they're approached with the player holding the cooresponding key
    private void OnCollisionEnter(Collision collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player == null) return;

        if (player.OwnKey(KeyName))
        {
            Destroy(gameObject);
        }
    }
}
