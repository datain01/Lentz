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
    public EnhancerAreaDetector enhancerAreaDetector;

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
    bool isPlacedOnEnhancer = false;

    Transform suctionChild = gameObject.transform.Find("Suction"); // Suction 자식 컴포넌트 찾기

    foreach (var targetArea in targetAreas)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(targetArea, eventData.position, eventData.pressEventCamera))
        {
            if (targetArea.CompareTag("Enhancer"))
            {
                isPlacedOnEnhancer = true;
                bool isLensMetered = lensDataManager.GetIsLensMetered(gameObject.tag);
                bool hasSuctionChild = suctionChild != null;
                
                
                if (isLensMetered && hasSuctionChild && !enhancerAreaDetector.IsLensAttached())
                {
                    rectTransform.anchoredPosition = targetArea.anchoredPosition;
                    GetComponent<LensSideChanger>()?.ChangeToSideView();
                    if (suctionChild.GetComponent<Image>() != null)
                    {
                        suctionChild.GetComponent<Image>().enabled = false; // Suction의 이미지 컴포넌트를 비활성화
                    }
                }
                // Enhancer에 달라붙지 않은 경우의 처리는 여기에 필요하지 않음
                return; // Enhancer 처리 후 for 루프 종료
            }
            else
            {
                // 다른 TargetArea에 달라붙은 경우
                GetComponent<LensSideChanger>()?.RestoreDefaultView();
                if (suctionChild && suctionChild.GetComponent<Image>() != null)
                {
                    suctionChild.GetComponent<Image>().enabled = true; // Suction의 이미지 컴포넌트를 다시 활성화
                }
            }

            rectTransform.anchoredPosition = targetArea.anchoredPosition;
            glassesController.SetLensPlaced(gameObject.tag);

            if (targetArea.CompareTag("TargetBase"))
            {
                isLensOnTargetBase = true;
                currentLensOnTargetBase = gameObject;
                lensDataManager.UpdateCurrentLens(gameObject);
                isPlacedOnTargetBase = true;
            }
            // TargetArea에 달라붙은 경우 처리 종료
            return;
        }
    }

    if (!isPlacedOnEnhancer || !lensDataManager.GetIsLensMetered(gameObject.tag))
    {
        GetComponent<LensSideChanger>()?.RestoreDefaultView();
        if (suctionChild && suctionChild.GetComponent<Image>() != null)
        {
            suctionChild.GetComponent<Image>().enabled = true; // Enhancer 영역 외부로 드래그한 경우 Suction의 이미지 컴포넌트를 활성화
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
