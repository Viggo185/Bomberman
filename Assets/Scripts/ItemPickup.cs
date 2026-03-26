using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        BombUp,
        FireUp,
        SpeedUp
    }
    public ItemType itemType;
    private void OnItemPickup(GameObject player)
    {
        switch (itemType)
        {
            case ItemType.BombUp:
                player.GetComponent<BombContrller>().AddBomb();
                break;
            case ItemType.FireUp:
                player.GetComponent<BombContrller>().explosionRange++;
                break;
            case ItemType.SpeedUp:
                player.GetComponent<MovementController>().speed ++;
                break;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnItemPickup(other.gameObject);
        }
    }
}
