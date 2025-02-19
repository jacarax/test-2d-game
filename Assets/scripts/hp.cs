using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Image healthBar; // ������ �� ������� ����������� HP ����
    public Image healthBarBackground; // ������ �� ������ ����������� ����
    private IHealth entity; // ������ �� ��������
    public float entityPosition_y=1.5f; // ������ ��� ���������
    void Start()
    {
        entity = GetComponentInParent<IHealth>(); // �������� ������ �� ������������ ������ � ����������� Entity
        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
        UpdateHPBarPosition();
    }

    void UpdateHealthBar()
    {
        if (entity != null)
        {
            healthBar.fillAmount = (float)entity.GetCurrentHealth() / entity.GetMaxHealth();
        }
    }

    void UpdateHPBarPosition()
    {
        // ��������� ������� HP ���� ��� ���������
        Vector3 entityPosition = transform.parent.position; // ������� ��������
        Vector3 hpBarPosition = new Vector3(entityPosition.x, entityPosition.y + entityPosition_y, entityPosition.z); // ������ ��� ���������
        transform.position = hpBarPosition; // ������������� ������� HP ����
    }
}



