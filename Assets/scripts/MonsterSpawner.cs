using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // Префаб монстра
    public Tilemap tilemap; // Ссылка на тайлмап
    public int numberOfMonsters = 5; // Количество монстров для спавна

    void Start()
    {
        SpawnMonsters();
    }

    void SpawnMonsters()
    {
        List<Vector3Int> validPositions = GetValidTilePositions();

        for (int i = 0; i < numberOfMonsters; i++)
        {
            if (validPositions.Count == 0) break; // Если нет доступных позиций, выходим из цикла

            int randomIndex = Random.Range(0, validPositions.Count);
            Vector3Int spawnPosition = validPositions[randomIndex];

            // Спавним монстра на случайной позиции
            Instantiate(monsterPrefab, tilemap.GetCellCenterWorld(spawnPosition), Quaternion.identity);

            // Удаляем использованную позицию из списка
            validPositions.RemoveAt(randomIndex);
        }
    }

    List<Vector3Int> GetValidTilePositions()
    {
        List<Vector3Int> positions = new List<Vector3Int>();

        // Проходим по всем плиткам тайлмапа
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.x; x < bounds.x + bounds.size.x; x++)
        {
            for (int y = bounds.y; y < bounds.y + bounds.size.y; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                // Проверяем, есть ли плитка на данной позиции
                if (tilemap.HasTile(position))
                {
                    positions.Add(position);
                }
            }
        }

        return positions;
    }
}
