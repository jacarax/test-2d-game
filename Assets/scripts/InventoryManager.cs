using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public class InventoryManager : MonoBehaviour
{

    public GameObject hp_Canvas; // Ссылка на ваш hp_Canvas
    public GameObject joystick_Canvas; // Ссылка на ваш joystick_Canvas

    public GameObject inventoryPanel; // Панель инвентаря
    public GameObject inventorySlotPrefab; // Префаб для ячейки инвентаря
  
    //public int numberOfSlots = 10; // Количество ячеек инвентаря
    public float spacing = 10f; // Расстояние между ячейками (можете изменить по своему усмотрению)
    
    private List<InventorySlot> inventorySlots = new List<InventorySlot>(); // Список ячеек инвентаря
    public float xPos2 = 200, yPos2 = -200; // расстояние от угла панели инвентаря
    public RemoveItemButtonHandler removeItemButtonHandler; // Ссылка на кнопку удаления
    public GameObject atack; // Ссылка на кнопку удаления
    public GameObject playerWithoutWeaponPrefab; // Префаб без оружия
    public GameObject playerWithWeaponPrefab;    // Префаб с оружием
    private GameObject currentPlayer;             // Текущий экземпляр персонажа

    public Vector3 offset;           // Смещение для префаба 
    void Start()
    {
        UpdatePlayer();
        inventoryPanel.SetActive(false); // Скрываем панель инвентаря в начале
       
        CreateInventorySlots(); // Создаем ячейки инвентаря
    }

    private void CreateInventorySlots()
    {

        RectTransform panelRect = inventoryPanel.GetComponent<RectTransform>(); // Получаем RectTransform панели
        float slotWidth = inventorySlotPrefab.GetComponent<RectTransform>().rect.width; // Ширина ячейки
        float slotHeight = inventorySlotPrefab.GetComponent<RectTransform>().rect.height; // Высота ячейки

        // Получаем размеры панели
        float panelWidth = panelRect.rect.width;
        float panelHeight = panelRect.rect.height;

        // Вычисляем количество ячеек, которые могут поместиться в панели
        int slotsPerRow = Mathf.FloorToInt(panelWidth / (slotWidth + spacing))-1; // Количество ячеек в строке
        int rows = Mathf.FloorToInt(panelHeight / (slotHeight + spacing))-1; // Количество строк

        // Максимальное количество ячеек, которое можно создать
        int totalSlots = slotsPerRow * rows;

        for (int i = 0; i < totalSlots; i++)
        {
            GameObject slotObject = Instantiate(inventorySlotPrefab, inventoryPanel.transform); // Создаем ячейку
            InventorySlot slot = slotObject.GetComponent<InventorySlot>(); // Получаем компонент InventorySlot

            // Вычисляем позицию ячейки
            int row = i / slotsPerRow; // Номер строки
            int column = i % slotsPerRow; // Номер столбца
            float xPos = column * (slotWidth + spacing); // X позиция с учетом расстояния
            float yPos = -row * (slotHeight + spacing); // Y позиция (отрицательная, чтобы располагать вниз)

            // Устанавливаем позицию ячейки
            RectTransform slotRect = slotObject.GetComponent<RectTransform>();
            slotRect.anchoredPosition = new Vector2(xPos + xPos2, yPos + yPos2); // Устанавливаем позицию относительно панели
            slot.removeItemButtonHandler = removeItemButtonHandler;
            slot.inventoryManager = this; // Передаем ссылку на текущий InventoryManager
            slot.currentItem = null;
            inventorySlots.Add(slot); // Добавляем ячейку в список
        }

    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(true); // Показываем панель инвентаря
        hp_Canvas.SetActive(false); // Отключаем hp_Canvas
        removeItemButtonHandler.gameObject.SetActive(false);
        atack.SetActive(false);
        joystick_Canvas.SetActive(false);
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false); // Скрываем панель инвентаря
        hp_Canvas.SetActive(true); // Включаем hp_Canvas
        removeItemButtonHandler.gameObject.SetActive(false);
        atack.SetActive(true);
        joystick_Canvas.SetActive(true);
    }


    public void ToggleInventory()
    {
        if (inventoryPanel.activeSelf)
        {
            CloseInventory(); // Закрываем инвентарь, если он открыт
        }
        else
        {
            OpenInventory(); // Открываем инвентарь, если он закрыт
        }
    }
    public void AddItem(Item item)
    {
        // Проверяем, можем ли мы добавить предмет в существующие ячейки
        foreach (InventorySlot slot in inventorySlots)
        {
            //if (slot.currentItem != null)
                if (slot.currentItem != null && slot.currentItem.currentAmount != slot.currentItem.maxStack && slot.currentItem.itemID == item.itemID)
                {
                    // Если предмет уже есть в ячейке, добавляем его
                    slot.AddItem(item);
                    return; // Выходим, если добавили предмет
                }
        }

        // Если предмет не был добавлен, ищем пустую ячейку
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.currentItem == null)
            {
                slot.AddItem(item); // Добавляем предмет в первую пустую ячейку
                UpdatePlayer();
                return; // Выходим, если добавили предмет
            }
        }
        
        // Если все ячейки заняты, можно добавить логику для обработки переполнения
        Debug.Log("Инвентарь полон!"); // Здесь можно добавить логику для обработки переполнения
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
            Destroy(currentPlayer); // Удаляем текущий объект персонажа
        }
        currentPlayer = Instantiate(newPlayerPrefab, transform.position+offset, transform.rotation, transform); // Создаем новый объект
    }


    public List<InventorySlot> GetInventorySlots()
    {
        return inventorySlots; // Возвращаем список ячеек инвентаря
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
            AddItem(itemData); // Добавляем предмет в инвентарь
        }
    }
}
