using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item; // Ссылка на объект Item, который будет добавлен в инвентарь

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, является ли объект игроком
        if (other.CompareTag("Player"))
        {
            // Получаем компонент InventoryManager у игрока
            InventoryManager inventory = other.GetComponent<InventoryManager>();
            if (inventory != null)
            {
                inventory.AddItem(item); // Добавляем предмет в инвентарь
                Destroy(gameObject); // Удаляем предмет из мира
            }
        }
    }
}
