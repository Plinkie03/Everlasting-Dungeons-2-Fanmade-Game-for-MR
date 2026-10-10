using Assets.EverlastingDungeons.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PadController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField]
    private Image image;
    private bool isPointerDown;
    [SerializeField]
    private Sprite up, left, down, right, idle;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerDown = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerDown = false;
        image.sprite = idle;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        image.sprite = idle;
    }

    private void Awake()
    {
        if (!Application.isMobilePlatform)
            gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isPointerDown)
        {
            PrefabManager.Instance.activePlayer.input.moveInput = Vector2.zero;
            return;
        }

        Vector2 screenPosition = Touchscreen.current != null
            ? Touchscreen.current.primaryTouch.position.ReadValue()
            : Mouse.current.position.ReadValue();

        RectTransform rect = image.rectTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect, screenPosition, null, out Vector2 localPoint
        );

        Vector2 normalized = new Vector2(
            localPoint.x / rect.rect.width,
            localPoint.y / rect.rect.height
        );

        float threshold = 0.1f;

        if (normalized.x > threshold)
        {
            image.sprite = right;
            PrefabManager.Instance.activePlayer.input.moveInput = Vector2.right;
        }
        else if (normalized.x < -threshold)
        {
            image.sprite = left;
            PrefabManager.Instance.activePlayer.input.moveInput = Vector2.left;
        }
        else if (normalized.y > threshold)
        {
            image.sprite = up;
            PrefabManager.Instance.activePlayer.input.moveInput = Vector2.up;
        }
        else if (normalized.y < -threshold)
        {
            image.sprite = down;
            PrefabManager.Instance.activePlayer.input.moveInput = Vector2.down;
        }
    }
}
