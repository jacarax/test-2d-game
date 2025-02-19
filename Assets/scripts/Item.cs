using UnityEngine;

[System.Serializable]
public class Item
{
    public int itemID;
    public string itemName; // Имя предмета
    public Sprite itemIcon; // Иконка предмета
    public int maxStack; // Максимальное количество в стаке
    public int currentAmount; // Текущее количество предметов
    public bool isWeapon;
   
}
