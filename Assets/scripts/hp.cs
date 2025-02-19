using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Image healthBar; // Ссылка на красное изображение HP бара
    public Image healthBarBackground; // Ссылка на черное изображение фона
    private IHealth entity; // Ссылка на сущность
    public float entityPosition_y=1.5f; // высота над сущностью
    void Start()
    {
        entity = GetComponentInParent<IHealth>(); // Получаем ссылку на родительский объект с компонентом Entity
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
        // Обновляем позицию HP бара над сущностью
        Vector3 entityPosition = transform.parent.position; // Позиция сущности
        Vector3 hpBarPosition = new Vector3(entityPosition.x, entityPosition.y + entityPosition_y, entityPosition.z); // Высота над сущностью
        transform.position = hpBarPosition; // Устанавливаем позицию HP бара
    }
}



