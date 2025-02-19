using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Enemy : MonoBehaviour, IHealth
{
    public float detectionRadius = 5f; // Радиус обнаружения
    public float attackRadius = 2f; // Радиус атаки
    public float moveSpeed = 2f; // Скорость движения
    public int damage = 20; // Урон, который наносит моб
    public float attackSpeed = 1f; // Скорость атаки (время между атаками)
    public GameObject itemDrop; // Префаб предмета, который будет выпадать
    private GameObject player; // Ссылка на игрока
    private float nextAttackTime = 0f; // Время следующей атаки

    public float maxHealth = 100;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player"); // Найти игрока по тегу
    }

    void Update()
    {
        // Проверка расстояния до игрока
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionRadius && distanceToPlayer > attackRadius)
        {
            // Движение к игроку
            MoveTowardsPlayer();
        }
        else if (distanceToPlayer <= attackRadius)
        {
            // Атака игрока
            AttackPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void AttackPlayer()
    {
        // Проверка времени для атаки
        if (Time.time >= nextAttackTime)
        {
            // Наносим урон игроку
            CharacterController playerScript = player.GetComponent<CharacterController>(); // Предполагается, что у игрока есть скрипт Player
            if (playerScript != null)
            {
                playerScript.TakeDamage(damage); // Наносим урон игроку
            }

            // Устанавливаем время следующей атаки
            nextAttackTime = Time.time + 1f / attackSpeed;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    void Die()
    {
        // Выпадение предмета
        Instantiate(itemDrop, transform.position, Quaternion.identity);
        Destroy(gameObject); // Уничтожение моба
    }
}
