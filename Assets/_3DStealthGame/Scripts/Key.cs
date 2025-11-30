using UnityEngine;
using UnityEngine.UI;


public class Key : MonoBehaviour
{
    public string KeyName;
    public Image Sprite;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        if (player == null) return;

        player.AddKey(KeyName);
        Destroy(gameObject);

        Sprite.enabled = true;
    }
}
