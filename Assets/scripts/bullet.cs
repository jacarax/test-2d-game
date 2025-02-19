using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Скорость пули
    public int damage = 10; // Урон, который наносит пуля
    private GameObject target; // Цель (моб)

    public void Initialize(GameObject target)
    {
        this.target = target;
        Destroy(gameObject, 2f); // Уничтожаем пулю через 2 секунды, если не попала в цель
    }

    void Update()
    {
        if (target != null)
        {
            // Двигаем пулю к цели
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Проверка на столкновение
            if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            {
                Enemy enemy = target.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage); // Наносим урон мобу
                }
                Destroy(gameObject); // Уничтожаем пулю
            }
        }
    }
}
