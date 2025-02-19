using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // ������ �������
    public Tilemap tilemap; // ������ �� �������
    public int numberOfMonsters = 5; // ���������� �������� ��� ������

    void Start()
    {
        SpawnMonsters();
    }

    void SpawnMonsters()
    {
        List<Vector3Int> validPositions = GetValidTilePositions();

        for (int i = 0; i < numberOfMonsters; i++)
        {
            if (validPositions.Count == 0) break; // ���� ��� ��������� �������, ������� �� �����

            int randomIndex = Random.Range(0, validPositions.Count);
            Vector3Int spawnPosition = validPositions[randomIndex];

            // ������� ������� �� ��������� �������
            Instantiate(monsterPrefab, tilemap.GetCellCenterWorld(spawnPosition), Quaternion.identity);

            // ������� �������������� ������� �� ������
            validPositions.RemoveAt(randomIndex);
        }
    }

    List<Vector3Int> GetValidTilePositions()
    {
        List<Vector3Int> positions = new List<Vector3Int>();

        // �������� �� ���� ������� ��������
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.x; x < bounds.x + bounds.size.x; x++)
        {
            for (int y = bounds.y; y < bounds.y + bounds.size.y; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                // ���������, ���� �� ������ �� ������ �������
                if (tilemap.HasTile(position))
                {
                    positions.Add(position);
                }
            }
        }

        return positions;
    }
}
