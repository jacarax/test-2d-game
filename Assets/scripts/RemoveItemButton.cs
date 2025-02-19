using UnityEngine;
using UnityEngine.UI;

public class RemoveItemButtonHandler : MonoBehaviour
{
    public InventorySlot currentItem; // ������ �� ������� ������ ���������

    public void OnRemoveButtonClick()
    {
        if (currentItem != null)
        {
            currentItem.RemoveItem(); // ����� ��� �������� ��������

            Debug.Log("������� ����� �� ���������");
        }
    }
}