using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Enemy : MonoBehaviour, IHealth
{
    public float detectionRadius = 5f; // ������ �����������
    public float attackRadius = 2f; // ������ �����
    public float moveSpeed = 2f; // �������� ��������
    public int damage = 20; // ����, ������� ������� ���
    public float attackSpeed = 1f; // �������� ����� (����� ����� �������)
    public GameObject itemDrop; // ������ ��������, ������� ����� ��������
    private GameObject player; // ������ �� ������
    private float nextAttackTime = 0f; // ����� ��������� �����

    public float maxHealth = 100;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player"); // ����� ������ �� ����
    }

    void Update()
    {
        // �������� ���������� �� ������
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionRadius && distanceToPlayer > attackRadius)
        {
            // �������� � ������
            MoveTowardsPlayer();
        }
        else if (distanceToPlayer <= attackRadius)
        {
            // ����� ������
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
        // �������� ������� ��� �����
        if (Time.time >= nextAttackTime)
        {
            // ������� ���� ������
            CharacterController playerScript = player.GetComponent<CharacterController>(); // ��������������, ��� � ������ ���� ������ Player
            if (playerScript != null)
            {
                playerScript.TakeDamage(damage); // ������� ���� ������
            }

            // ������������� ����� ��������� �����
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
        // ��������� ��������
        Instantiate(itemDrop, transform.position, Quaternion.identity);
        Destroy(gameObject); // ����������� ����
    }
}
