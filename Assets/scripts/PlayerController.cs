using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class CharacterController : MonoBehaviour, IHealth
{
    public float moveSpeed = 5f;

    public float maxHealth = 100;
    public float currentHealth = 100;


    private Rigidbody2D rb;
    public VirtualJoystick joystick; // Ссылка на ваш джойстик

    public GameObject bulletPrefab; // Префаб пули
    public Transform bulletSpawnPoint; // Точка спавна пули

    
    public InventoryManager inventoryManager; // Ссылка на InventoryManager


    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        Vector2 movement = joystick.GetInput(); // Получаем ввод от джойстика
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
        // Логика для уничтожения объекта или проигрывания анимации смерти
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
        // Проверяем, есть ли оружие в инвентаре
        if (inventoryManager.HasWeapon())
        {
            // Проверяем, есть ли патроны в инвентаре
            foreach (InventorySlot slot in inventoryManager.GetInventorySlots())
            {
                if (slot.currentItem != null && slot.currentItem.itemID == 2 && slot.currentItem.currentAmount > 0)
                {
                    // Находим ближайшего моба с тегом "Monster"
                    GameObject targetMonster = FindClosestMonster();

                    if (targetMonster != null)
                    {
                        // Создаем пулю
                        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                        Bullet bulletScript = bullet.GetComponent<Bullet>();
                        bulletScript.Initialize(targetMonster); // Передаем найденного моба

                        // Уменьшаем количество патронов в инвентаре
                        slot.RemoveItem(); // Удаляем один патрон
                        return; // Выходим, если стрельба произошла
                    }
                    else
                    {
                        Debug.Log("Нет мобов для стрельбы!"); // Сообщение, если мобов нет
                    }
                }
            }
        }

        Debug.Log("Нет патронов для стрельбы!"); // Сообщение, если патронов нет
    }
    
}