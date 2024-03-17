using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform[] targetAreas; // 여러 Target Area를 위한 배열
    public RectTransform targetBaseArea; // TargetBase 영역

    private Canvas parentCanvas;
    private RectTransform canvasRectTransform;
    private RectTransform rectTransform;
    private Vector2 originalPosition;

    public GlassesController glassesController; // 인스펙터에서 할당
    public LensDataManager lensDataManager; // 인스펙터에서 할당

    public static bool isLensOnTargetBase = false;
    public static GameObject currentLensOnTargetBase; // TargetBase에 올려진 렌즈의 참조

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
        canvasRectTransform = parentCanvas.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition3D;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, eventData.position, eventData.pressEventCamera, out pos);
        rectTransform.anchoredPosition = pos;

        // 드래그 중 TargetBase 영역 확인 및 처리
        CheckLensPlacementDuringDrag();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool isPlacedOnTargetBase = false;

        foreach (var targetArea in targetAreas)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(targetArea, eventData.position, eventData.pressEventCamera))
            {
                rectTransform.anchoredPosition = targetArea.anchoredPosition;
                glassesController.SetLensPlaced(gameObject.tag);

                if (targetArea.CompareTag("TargetBase"))
                {
                    isLensOnTargetBase = true;
                    currentLensOnTargetBase = gameObject;
                    lensDataManager.UpdateCurrentLens(gameObject);
                    isPlacedOnTargetBase = true;
                }
                break;
            }
        }

        if (!isPlacedOnTargetBase && currentLensOnTargetBase == gameObject)
        {
            lensDataManager.UpdateCurrentLens(null);
            isLensOnTargetBase = false;
            currentLensOnTargetBase = null;
        }
    }

    private void CheckLensPlacementDuringDrag()
    {
        // 드래그 중 TargetBase 영역 내에 있는지 확인
        bool isWithinTargetBase = RectTransformUtility.RectangleContainsScreenPoint(targetBaseArea, rectTransform.position, null);

        if (isWithinTargetBase)
        {
            lensDataManager.UpdateCurrentLens(gameObject);
            isLensOnTargetBase = true;
            currentLensOnTargetBase = gameObject;
        }
        else
        {
            if (currentLensOnTargetBase == gameObject)
            {
                lensDataManager.UpdateCurrentLens(null);
                isLensOnTargetBase = false;
                currentLensOnTargetBase = null;
            }
        }
    }
}
