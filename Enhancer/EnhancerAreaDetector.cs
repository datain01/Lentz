using UnityEngine;
using UnityEngine.UI;

public class EnhancerAreaDetector : MonoBehaviour
{
    public EnhancerHandle enhancerHandle;
    public EnhancerTextUIUpdater enhancerTextUIUpdater;
    public Button[] enhanceButtons; 
    public GameObject attachedLens = null; 

    private void Awake()
    {
        SetIncreaseButtonsActive(false);
    }

     private void SetIncreaseButtonsActive(bool isActive)
    {
        foreach (var button in enhanceButtons)
        {
            if (button != null) // 버튼이 할당되었는지 확인
            {
                button.interactable = isActive; // 버튼의 interactable 속성 조정
            }
        }
    }

    public bool IsLensAttached()
    {
        return attachedLens != null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("LensLeft") || other.CompareTag("LensRight")) && attachedLens == null)
        {
            attachedLens = other.gameObject; // 달라붙은 렌즈 설정
            enhancerHandle.SetButtonActive(false); // EnhancerHandle 버튼 비활성화
            SetIncreaseButtonsActive(true); // 확률 증가 버튼 활성화

            // 렌즈의 Lightrical 값을 가져와 UI에 표시
            LensDataManager lensDataManager = LensDataManager.Instance;
            if (lensDataManager != null)
            {
                LensData lensData = null;
                if (other.CompareTag("LensLeft"))
                {
                    lensData = lensDataManager.LensDataLeft;
                    lensDataManager.UpdateCurrentLens(attachedLens); 
                }
                else if (other.CompareTag("LensRight"))
                {
                    lensData = lensDataManager.LensDataRight;
                    lensDataManager.UpdateCurrentLens(attachedLens); 
                }

                // Lightrical 값에 따른 강화 버튼 활성화/비활성화
                if (lensData.Lightrical >= 8)
                {
                    // Lightrical 값이 8 이상이면 강화 버튼 비활성화
                    SetIncreaseButtonsActive(false);
                }
                else
                {
                    // 그렇지 않으면 강화 버튼 활성화
                    SetIncreaseButtonsActive(true);
                }

                enhancerTextUIUpdater.UpdateLensInfo(lensData.Lightrical, 0);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == attachedLens)
        {
            attachedLens = null; // 달라붙은 렌즈 초기화
            enhancerHandle.SetButtonActive(true); // EnhancerHandle 버튼 활성화
            SetIncreaseButtonsActive(false);

            if (EnhancerCalculator.Instance != null)
            {
                EnhancerCalculator.Instance.pendingLentzUsage = 0;
                // EnhancerCalculator.Instance.UpdateProbabilityUI(); // UI도 업데이트 해줘야 할 수 있음
            }
            
            enhancerTextUIUpdater.UpdateLensInfo(0, 0);; // 예시: 기본값으로 리셋
        }
    }
}
