using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public CharacterController player; // ������ �� ������ ������
    public InventoryManager inventoryManager; // ������ �� ������ ������

    private void Start()
    {
        LoadPlayerData(); // ��������� ������ ������ ��� ������
    }

    public void SavePlayerData()
    {
        GameData gameData = new GameData
        {
            playerData = new PlayerData
            {
                position = player.transform.position,
                health = player.GetCurrentHealth(),
            },
            inventoryItems = inventoryManager.GetInventoryData() // �������� ������ ���������
        };

        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(Application.persistentDataPath + "/playerSave.json", json);
        Debug.Log("������ ������ ���������!");
    }

    public void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playerSave.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameData gameData = JsonUtility.FromJson<GameData>(json);

            // ��������������� ������ ������
            player.transform.position = gameData.playerData.position;
            player.TakeDamage(player.GetMaxHealth() - gameData.playerData.health); // ��������������� ��������

            // ��������������� ������ ���������
            inventoryManager.SetInventoryData(gameData.inventoryItems);

            Debug.Log("������ ������ ���������!");
        }
        else
        {
            Debug.LogError("���� ���������� �� ������!");
        }
    }
}
