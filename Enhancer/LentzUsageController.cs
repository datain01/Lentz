using UnityEngine;
using TMPro;

public class LentzUsageController : MonoBehaviour
{
    public LentzManager lentzManager;
    public EnhancerCalculator enhancerCalculator;
    public EnhancerTextUIUpdater enhancerTextUIUpdater;
    
    // 렌츠 사용량 적용 및 확률 업데이트
    public void ApplyLentzUsage(int amount)
    {
        if (lentzManager.lentzAmount >= amount) // 사용 가능한 렌츠가 충분한지 확인
        {
            lentzManager.UseLentz(amount); // 렌츠 사용량 적용
            // 강화 확률 업데이트 로직 필요 시 여기에 추가
            UpdateEnhancementProbability(amount);
        }
        else
        {
            Debug.Log("Not enough Lentz to use");
        }
    }

    // 강화 확률 업데이트
    void UpdateEnhancementProbability(int usedLentz)
    {
        LensDataManager lensDataManager = LensDataManager.Instance;
        if (lensDataManager.CurrentLens != null)
        {
            LensData lensData = lensDataManager.CurrentLens.tag == "LensLeft" ? lensDataManager.LensDataLeft : lensDataManager.LensDataRight;
            // 강화 확률 계산
            float enhancementProbability = enhancerCalculator.CalculateEnhancementProbability(lensData.Spherical, lensData.Cylindrical, lensData.Lightrical, usedLentz);
            int roundedProbability = Mathf.CeilToInt(enhancementProbability);
            // 계산된 확률을 화면에 표시
            enhancerTextUIUpdater.UpdateLensInfo(lensData.Lightrical, roundedProbability);
        }
    }
}

