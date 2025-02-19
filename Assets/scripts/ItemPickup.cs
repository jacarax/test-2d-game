using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item; // ������ �� ������ Item, ������� ����� �������� � ���������

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���������, �������� �� ������ �������
        if (other.CompareTag("Player"))
        {
            // �������� ��������� InventoryManager � ������
            InventoryManager inventory = other.GetComponent<InventoryManager>();
            if (inventory != null)
            {
                inventory.AddItem(item); // ��������� ������� � ���������
                Destroy(gameObject); // ������� ������� �� ����
            }
        }
    }
}
