using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon; // ����������� ��� ����������� ������ ��������
    public Item currentItem; // ������� ������� � ������
    public TextMeshProUGUI quantityText; // ����� ��� ����������� ���������� ���������
    public RemoveItemButtonHandler removeItemButtonHandler; // ������ �� ������ ��������
    public InventoryManager inventoryManager; // ������ �� InventoryManager
    public void AddItem(Item item)
    {
        if (currentItem == null)
        {
            // ���� ������ �����, ��������� �������
            currentItem = item;
            
            icon.sprite = item.itemIcon; // ������������� ������
            icon.enabled = true; // ���������� ������
        }
        else if (currentItem.itemID == item.itemID)
        {
            // ���� ������� ��� ���� � ������, ��������� � ����������
            int totalAmount = currentItem.currentAmount + item.currentAmount;

            if (totalAmount <= currentItem.maxStack)
            {
                currentItem.currentAmount = totalAmount; // ��������� ����������
            }
            else
            {
                // ���� ��������� ������������ ����, ��������� ������ �� ��������� � ���������� �������
                int overflow = totalAmount - currentItem.maxStack;
                currentItem.currentAmount = currentItem.maxStack; // ��������� �� ���������
                // ��������� ����� ����������
                item.currentAmount=overflow;
                inventoryManager.AddItem(item);
                // ����� ���������� �������� ������ ��� �������� ����� ������ ��� �������
                // ��������, �� ������ ������� ����� � InventoryManager ��� ���������� ������ ��������
                
            }

        }
        if (currentItem.maxStack != 1)
            quantityText.text = currentItem.currentAmount.ToString();// ��������� ����� ����������
    }

    public void ClearSlot()
    {
        currentItem = null; // ������� ������� �������
        icon.sprite = null; // ������� ������
        icon.enabled = false; // �������� ������
        quantityText.text = ""; // ������� ����� ����������
        
}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem != null && removeItemButtonHandler != null)
        {
            removeItemButtonHandler.currentItem = this; // ������������� ������� ������
            removeItemButtonHandler.gameObject.SetActive(true); // ���������� ������
        }
    }

    public void RemoveItem()
    {
        if (currentItem.itemIcon != null)
        {
            currentItem.currentAmount--;

            if (currentItem.currentAmount <= 0)
            {
                ClearSlot(); // ������� ������, ���� ���������� ��������� 0
                inventoryManager.UpdatePlayer();
            }
            else
            {
                quantityText.text = currentItem.currentAmount.ToString(); // ��������� ����� ����������
            }
        }
    }
}
