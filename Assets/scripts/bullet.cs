using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // �������� ����
    public int damage = 10; // ����, ������� ������� ����
    private GameObject target; // ���� (���)

    public void Initialize(GameObject target)
    {
        this.target = target;
        Destroy(gameObject, 2f); // ���������� ���� ����� 2 �������, ���� �� ������ � ����
    }

    void Update()
    {
        if (target != null)
        {
            // ������� ���� � ����
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // �������� �� ������������
            if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            {
                Enemy enemy = target.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage); // ������� ���� ����
                }
                Destroy(gameObject); // ���������� ����
            }
        }
    }
}
