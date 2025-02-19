using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon; // Изображение для отображения иконки предмета
    public Item currentItem; // Текущий предмет в ячейке
    public TextMeshProUGUI quantityText; // Текст для отображения количества предметов
    public RemoveItemButtonHandler removeItemButtonHandler; // Ссылка на кнопку удаления
    public InventoryManager inventoryManager; // Ссылка на InventoryManager
    public void AddItem(Item item)
    {
        if (currentItem == null)
        {
            // Если ячейка пуста, добавляем предмет
            currentItem = item;
            
            icon.sprite = item.itemIcon; // Устанавливаем иконку
            icon.enabled = true; // Показываем иконку
        }
        else if (currentItem.itemID == item.itemID)
        {
            // Если предмет уже есть в ячейке, добавляем к количеству
            int totalAmount = currentItem.currentAmount + item.currentAmount;

            if (totalAmount <= currentItem.maxStack)
            {
                currentItem.currentAmount = totalAmount; // Обновляем количество
            }
            else
            {
                // Если превышаем максимальный стак, заполняем ячейку до максимума и возвращаем остаток
                int overflow = totalAmount - currentItem.maxStack;
                currentItem.currentAmount = currentItem.maxStack; // Заполняем до максимума
                // Обновляем текст количества
                item.currentAmount=overflow;
                inventoryManager.AddItem(item);
                // Здесь необходимо добавить логику для создания новой ячейки для остатка
                // Например, вы можете вызвать метод в InventoryManager для добавления нового предмета
                
            }

        }
        if (currentItem.maxStack != 1)
            quantityText.text = currentItem.currentAmount.ToString();// Обновляем текст количества
    }

    public void ClearSlot()
    {
        currentItem = null; // Очищаем текущий предмет
        icon.sprite = null; // Убираем иконку
        icon.enabled = false; // Скрываем иконку
        quantityText.text = ""; // Очищаем текст количества
        
}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem != null && removeItemButtonHandler != null)
        {
            removeItemButtonHandler.currentItem = this; // Устанавливаем текущую ячейку
            removeItemButtonHandler.gameObject.SetActive(true); // Активируем кнопку
        }
    }

    public void RemoveItem()
    {
        if (currentItem.itemIcon != null)
        {
            currentItem.currentAmount--;

            if (currentItem.currentAmount <= 0)
            {
                ClearSlot(); // Очищаем ячейку, если количество предметов 0
                inventoryManager.UpdatePlayer();
            }
            else
            {
                quantityText.text = currentItem.currentAmount.ToString(); // Обновляем текст количества
            }
        }
    }
}
