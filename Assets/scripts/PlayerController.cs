using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class CharacterController : MonoBehaviour, IHealth
{
    public float moveSpeed = 5f;

    public float maxHealth = 100;
    public float currentHealth = 100;


    private Rigidbody2D rb;
    public VirtualJoystick joystick; // ������ �� ��� ��������

    public GameObject bulletPrefab; // ������ ����
    public Transform bulletSpawnPoint; // ����� ������ ����

    
    public InventoryManager inventoryManager; // ������ �� InventoryManager


    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        Vector2 movement = joystick.GetInput(); // �������� ���� �� ���������
        rb.velocity = movement * moveSpeed;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        // ������ ��� ����������� ������� ��� ������������ �������� ������
        Destroy(gameObject);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }


    GameObject FindClosestMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject closestMonster = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject monster in monsters)
        {
            float distance = Vector3.Distance(transform.position, monster.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestMonster = monster;
            }
        }

        return closestMonster;
    }
    public void Shoot()
    {
        // ���������, ���� �� ������ � ���������
        if (inventoryManager.HasWeapon())
        {
            // ���������, ���� �� ������� � ���������
            foreach (InventorySlot slot in inventoryManager.GetInventorySlots())
            {
                if (slot.currentItem != null && slot.currentItem.itemID == 2 && slot.currentItem.currentAmount > 0)
                {
                    // ������� ���������� ���� � ����� "Monster"
                    GameObject targetMonster = FindClosestMonster();

                    if (targetMonster != null)
                    {
                        // ������� ����
                        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                        Bullet bulletScript = bullet.GetComponent<Bullet>();
                        bulletScript.Initialize(targetMonster); // �������� ���������� ����

                        // ��������� ���������� �������� � ���������
                        slot.RemoveItem(); // ������� ���� ������
                        return; // �������, ���� �������� ���������
                    }
                    else
                    {
                        Debug.Log("��� ����� ��� ��������!"); // ���������, ���� ����� ���
                    }
                }
            }
        }

        Debug.Log("��� �������� ��� ��������!"); // ���������, ���� �������� ���
    }
    
}