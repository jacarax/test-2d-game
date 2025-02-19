using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public CharacterController player; // Ссылка на вашего игрока
    public InventoryManager inventoryManager; // Ссылка на вашего игрока

    private void Start()
    {
        LoadPlayerData(); // Загружаем данные игрока при старте
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
            inventoryItems = inventoryManager.GetInventoryData() // Получаем данные инвентаря
        };

        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(Application.persistentDataPath + "/playerSave.json", json);
        Debug.Log("Данные игрока сохранены!");
    }

    public void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playerSave.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameData gameData = JsonUtility.FromJson<GameData>(json);

            // Восстанавливаем данные игрока
            player.transform.position = gameData.playerData.position;
            player.TakeDamage(player.GetMaxHealth() - gameData.playerData.health); // Восстанавливаем здоровье

            // Восстанавливаем данные инвентаря
            inventoryManager.SetInventoryData(gameData.inventoryItems);

            Debug.Log("Данные игрока загружены!");
        }
        else
        {
            Debug.LogError("Файл сохранения не найден!");
        }
    }
}
