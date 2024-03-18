using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragPreventCanvas : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 끝났을 때의 추가적인 로직이 필요하다면 여기에 구현
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canvas) return;

        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition))
        {
            Vector2 clampedPosition = ClampToCanvas(localPointerPosition);
            rectTransform.anchoredPosition = clampedPosition;
        }
    }

    private Vector2 ClampToCanvas(Vector2 localPosition)
    {
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;
        float clampedX = Mathf.Clamp(localPosition.x, -canvasSize.x / 2, canvasSize.x / 2);
        float clampedY = Mathf.Clamp(localPosition.y, -canvasSize.y / 2, canvasSize.y / 2);

        return new Vector2(clampedX, clampedY);
    }
}