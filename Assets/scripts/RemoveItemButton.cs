using UnityEngine;
using UnityEngine.UI;

public class RemoveItemButtonHandler : MonoBehaviour
{
    public InventorySlot currentItem; // Ссылка на текущую ячейку инвентаря

    public void OnRemoveButtonClick()
    {
        if (currentItem != null)
        {
            currentItem.RemoveItem(); // Метод для удаления предмета

            Debug.Log("Предмет удалён из инвентаря");
        }
    }
}