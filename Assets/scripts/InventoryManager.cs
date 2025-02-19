using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public class InventoryManager : MonoBehaviour
{

    public GameObject hp_Canvas; // ������ �� ��� hp_Canvas
    public GameObject joystick_Canvas; // ������ �� ��� joystick_Canvas

    public GameObject inventoryPanel; // ������ ���������
    public GameObject inventorySlotPrefab; // ������ ��� ������ ���������
  
    //public int numberOfSlots = 10; // ���������� ����� ���������
    public float spacing = 10f; // ���������� ����� �������� (������ �������� �� ������ ����������)
    
    private List<InventorySlot> inventorySlots = new List<InventorySlot>(); // ������ ����� ���������
    public float xPos2 = 200, yPos2 = -200; // ���������� �� ���� ������ ���������
    public RemoveItemButtonHandler removeItemButtonHandler; // ������ �� ������ ��������
    public GameObject atack; // ������ �� ������ ��������
    public GameObject playerWithoutWeaponPrefab; // ������ ��� ������
    public GameObject playerWithWeaponPrefab;    // ������ � �������
    private GameObject currentPlayer;             // ������� ��������� ���������

    public Vector3 offset;           // �������� ��� ������� 
    void Start()
    {
        UpdatePlayer();
        inventoryPanel.SetActive(false); // �������� ������ ��������� � ������
       
        CreateInventorySlots(); // ������� ������ ���������
    }

    private void CreateInventorySlots()
    {

        RectTransform panelRect = inventoryPanel.GetComponent<RectTransform>(); // �������� RectTransform ������
        float slotWidth = inventorySlotPrefab.GetComponent<RectTransform>().rect.width; // ������ ������
        float slotHeight = inventorySlotPrefab.GetComponent<RectTransform>().rect.height; // ������ ������

        // �������� ������� ������
        float panelWidth = panelRect.rect.width;
        float panelHeight = panelRect.rect.height;

        // ��������� ���������� �����, ������� ����� ����������� � ������
        int slotsPerRow = Mathf.FloorToInt(panelWidth / (slotWidth + spacing))-1; // ���������� ����� � ������
        int rows = Mathf.FloorToInt(panelHeight / (slotHeight + spacing))-1; // ���������� �����

        // ������������ ���������� �����, ������� ����� �������
        int totalSlots = slotsPerRow * rows;

        for (int i = 0; i < totalSlots; i++)
        {
            GameObject slotObject = Instantiate(inventorySlotPrefab, inventoryPanel.transform); // ������� ������
            InventorySlot slot = slotObject.GetComponent<InventorySlot>(); // �������� ��������� InventorySlot

            // ��������� ������� ������
            int row = i / slotsPerRow; // ����� ������
            int column = i % slotsPerRow; // ����� �������
            float xPos = column * (slotWidth + spacing); // X ������� � ������ ����������
            float yPos = -row * (slotHeight + spacing); // Y ������� (�������������, ����� ����������� ����)

            // ������������� ������� ������
            RectTransform slotRect = slotObject.GetComponent<RectTransform>();
            slotRect.anchoredPosition = new Vector2(xPos + xPos2, yPos + yPos2); // ������������� ������� ������������ ������
            slot.removeItemButtonHandler = removeItemButtonHandler;
            slot.inventoryManager = this; // �������� ������ �� ������� InventoryManager
            slot.currentItem = null;
            inventorySlots.Add(slot); // ��������� ������ � ������
        }

    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(true); // ���������� ������ ���������
        hp_Canvas.SetActive(false); // ��������� hp_Canvas
        removeItemButtonHandler.gameObject.SetActive(false);
        atack.SetActive(false);
        joystick_Canvas.SetActive(false);
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false); // �������� ������ ���������
        hp_Canvas.SetActive(true); // �������� hp_Canvas
        removeItemButtonHandler.gameObject.SetActive(false);
        atack.SetActive(true);
        joystick_Canvas.SetActive(true);
    }


    public void ToggleInventory()
    {
        if (inventoryPanel.activeSelf)
        {
            CloseInventory(); // ��������� ���������, ���� �� ������
        }
        else
        {
            OpenInventory(); // ��������� ���������, ���� �� ������
        }
    }
    public void AddItem(Item item)
    {
        // ���������, ����� �� �� �������� ������� � ������������ ������
        foreach (InventorySlot slot in inventorySlots)
        {
            //if (slot.currentItem != null)
                if (slot.currentItem != null && slot.currentItem.currentAmount != slot.currentItem.maxStack && slot.currentItem.itemID == item.itemID)
                {
                    // ���� ������� ��� ���� � ������, ��������� ���
                    slot.AddItem(item);
                    return; // �������, ���� �������� �������
                }
        }

        // ���� ������� �� ��� ��������, ���� ������ ������
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.currentItem == null)
            {
                slot.AddItem(item); // ��������� ������� � ������ ������ ������
                UpdatePlayer();
                return; // �������, ���� �������� �������
            }
        }
        
        // ���� ��� ������ ������, ����� �������� ������ ��� ��������� ������������
        Debug.Log("��������� �����!"); // ����� ����� �������� ������ ��� ��������� ������������
    }

    public void UpdatePlayer()
    {
        if (HasWeapon())
        {
            SwitchToPlayer(playerWithWeaponPrefab);
        }
        else if (!HasWeapon())
        {
            SwitchToPlayer(playerWithoutWeaponPrefab);
        }
    }

    void SwitchToPlayer(GameObject newPlayerPrefab)
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer); // ������� ������� ������ ���������
        }
        currentPlayer = Instantiate(newPlayerPrefab, transform.position+offset, transform.rotation, transform); // ������� ����� ������
    }


    public List<InventorySlot> GetInventorySlots()
    {
        return inventorySlots; // ���������� ������ ����� ���������
    }

    public bool HasWeapon()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.currentItem !=null &&slot.currentItem.isWeapon)
            {
                return true;
            }
        }
        return false;
    }
    public List<Item> GetInventoryData()
    {
        List<Item> items = new List<Item>();
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.currentItem != null)
            {
                items.Add(slot.currentItem);
                
            }
        }
        return items;
    }

    public void SetInventoryData(List<Item> itemsData)
    {
        foreach (Item itemData in itemsData)
        {
            AddItem(itemData); // ��������� ������� � ���������
        }
    }
}
