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

    public void OnDrag(PointerEventData eventData)
    {
        if (!canvas) return;

        Vector2 position = eventData.position;
        Vector2 clampedPosition = ClampToCanvas(position);
        rectTransform.anchoredPosition = clampedPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 끝났을 때의 추가적인 로직이 필요하다면 여기에 구현
    }

    private Vector2 ClampToCanvas(Vector2 position)
    {
        Vector3[] canvasCorners = new Vector3[4];
        canvas.GetComponent<RectTransform>().GetWorldCorners(canvasCorners);

        float clampedX = Mathf.Clamp(position.x, canvasCorners[0].x, canvasCorners[2].x);
        float clampedY = Mathf.Clamp(position.y, canvasCorners[0].y, canvasCorners[2].y);

        Vector2 newScreenPosition = new Vector2(clampedX, clampedY);
        Vector2 newPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), newScreenPosition, canvas.worldCamera, out newPosition);

        return newPosition;
    }
}