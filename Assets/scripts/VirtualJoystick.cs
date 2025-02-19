using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform joystickBackground; // Фон джойстика
    public RectTransform joystickHandle; // Ручка джойстика

    private Vector2 inputVector;

    public Vector2 GetInput()
    {
        return inputVector;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        // Получаем положение нажатия относительно фона джойстика
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, eventData.position, eventData.pressEventCamera, out position);

        // Нормализуем координаты
        position.x = (position.x / joystickBackground.sizeDelta.x);
        position.y = (position.y / joystickBackground.sizeDelta.y);

        // Устанавливаем координаты ввода
        inputVector = new Vector2(position.x * 2 - 1, position.y * 2 - 1);
        inputVector = (inputVector.magnitude > 1) ? inputVector.normalized : inputVector;

        // Перемещаем ручку джойстика
        joystickHandle.anchoredPosition = new Vector2(inputVector.x * (joystickBackground.sizeDelta.x / 2), inputVector.y * (joystickBackground.sizeDelta.y / 2));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
    }
}

