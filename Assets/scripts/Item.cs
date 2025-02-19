using UnityEngine;

[System.Serializable]
public class Item
{
    public int itemID;
    public string itemName; // ��� ��������
    public Sprite itemIcon; // ������ ��������
    public int maxStack; // ������������ ���������� � �����
    public int currentAmount; // ������� ���������� ���������
    public bool isWeapon;
   
}
