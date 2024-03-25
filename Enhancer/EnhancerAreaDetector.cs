using UnityEngine;

public class EnhancerAreaDetector : MonoBehaviour
{
    public EnhancerHandle enhancerHandle;
    public EnhancerTextUIUpdater enhancerTextUIUpdater;
    private GameObject attachedLens = null; // 현재 Enhancer에 달라붙은 렌즈

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

                // OnTriggerEnter2D 메서드 내부에서
                if (lensData != null)
                {
                    // Lightrical 값을 표시하고, 초기 강화 확률(예시로 0을 사용)을 함께 표시
                    enhancerTextUIUpdater.UpdateLensInfo(lensData.Lightrical, 0);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == attachedLens)
        {
            attachedLens = null; // 달라붙은 렌즈 초기화
            enhancerHandle.SetButtonActive(true); // EnhancerHandle 버튼 활성화
            // 렌즈가 영역을 벗어났으므로 UI를 초기화하거나 기본값으로 설정
            enhancerTextUIUpdater.UpdateLensInfo(0, 0);; // 예시: 기본값으로 리셋
        }
    }
}
