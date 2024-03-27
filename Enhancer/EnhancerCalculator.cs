using UnityEngine;
using UnityEngine.UI;


public class EnhancerCalculator : MonoBehaviour
{
    public EnhancerAreaDetector enhancerAreaDetector;
    public EnhancerTextUIUpdater enhancerTextUIUpdater;

    public int CurrentProbability { get; set; }
    private bool hasCalculatedProbability = false;
    public int pendingLentzUsage = 0;
    public int totalUsedLentz = 0;

    [SerializeField]
    private Button[] enhanceButtons; // Unity Inspector에서 할당



    void Update()
    {
        if (enhancerAreaDetector.IsLensAttached() && !hasCalculatedProbability)
        {
            ApplyEnhancement();
            hasCalculatedProbability = true; // 확률 계산을 했다는 표시를 남깁니다.
        }
        else if (!enhancerAreaDetector.IsLensAttached())
        {
            hasCalculatedProbability = false; // 렌즈가 감지되지 않으면 다시 계산할 준비를 합니다.
        }
    }

    public void PrepareLentzUsage(int amount)
    {
        pendingLentzUsage += amount;
        // UI 업데이트 로직 추가 (확률 표시 업데이트 필요 없음)
    }

    public void TryEnhanceLightrical()
    {
        if (!enhancerAreaDetector.IsLensAttached() || pendingLentzUsage > LentzManager.Instance.lentzAmount) return;

        // 렌츠 사용을 진행하고 확률을 재계산합니다.
        LentzManager.Instance.UseLentz(pendingLentzUsage);
        LensData currentLensData = GetCurrentLensData();
        if (currentLensData != null)
        {
            int randomChance = Random.Range(0, 100);
            if (randomChance < CurrentProbability)
            {
                Debug.Log("Enhancement successful!");
                currentLensData.IncreaseLightrical();
            }
            else
            {
                Debug.Log("Enhancement failed.");
            }

            // 강화 시도 후 사용된 렌츠 수와 확률을 리셋합니다.
            pendingLentzUsage = 0;
            CurrentProbability = CalculateEnhancementProbability(currentLensData.Spherical, currentLensData.Cylindrical, currentLensData.Lightrical, 0);

            if (currentLensData.Lightrical >= 8)
            {
                foreach (var button in enhanceButtons){
                button.interactable = false; // 경휘가 8이상이면 버튼 비활성화
                } 
                Debug.Log("Lightrical reached maximum value. Enhancement disabled.");
            }
            }
            else
            {
                // 강화 실패
                Debug.Log("Enhancement failed.");
            }
    }



     private LensData GetCurrentLensData()
    {
        if (LensDataManager.Instance.CurrentLens?.tag == "LensLeft")
        {
            return LensDataManager.Instance.LensDataLeft;
        }
        else if (LensDataManager.Instance.CurrentLens?.tag == "LensRight")
        {
            return LensDataManager.Instance.LensDataRight;
        }
        return null;
    }

    void ApplyEnhancement()
    {
        LensData currentLensData = GetCurrentLensData();
        if (currentLensData != null)
        {
            Debug.Log($"Lens Data - Spherical: {currentLensData.Spherical}, Cylindrical: {currentLensData.Cylindrical}, Lightrical: {currentLensData.Lightrical}");
            CurrentProbability = CalculateEnhancementProbability(currentLensData.Spherical, currentLensData.Cylindrical, currentLensData.Lightrical, 0);
            enhancerTextUIUpdater.UpdateLensInfo(currentLensData.Lightrical, CurrentProbability);
        }
    }


    // 강화 확률 계산 메서드
    public int CalculateEnhancementProbability(float spherical, float cylindrical, int lightrical, int usedLentz)
    {
        if (lightrical >= 8)
        {
            return 0;
        }

        float baseProbability = GetBaseProbability(lightrical) - GetSphericalAdjustment(spherical) - GetCylindricalAdjustment(cylindrical) + GetLentzAdjustment(usedLentz);
        return Mathf.Clamp(Mathf.CeilToInt(baseProbability), 0, 100);
    }

    public void IncreaseProbability(int increaseAmount)
    {
        // 실제 증가시킬 수 있는 최대 확률 계산
        int actualIncreaseAmount = Mathf.Min(increaseAmount, 100 - CurrentProbability);

        // 실제 증가시킬 수 있는 확률이 0보다 클 때만 작업 수행
        if (actualIncreaseAmount > 0)
        {
            // 렌츠가 충분한지 확인
            if (actualIncreaseAmount > 0 && LentzManager.Instance.lentzAmount >= actualIncreaseAmount)
            {
                LentzManager.Instance.UseLentz(actualIncreaseAmount);
                CurrentProbability += actualIncreaseAmount;
                totalUsedLentz += actualIncreaseAmount; // 소모한 렌츠 양 업데이트
                UpdateProbabilityUI();
            }
            else
            {
                Debug.Log("Not enough Lentz to increase probability.");
            }
        }
        else
        {
            Debug.Log("Probability is already at or exceeds 100%.");
        }
    }

    public void OnIncreaseProbabilityByOne()
    {
        IncreaseProbability(1); // 확률을 1만큼 증가
    }

    public void OnIncreaseProbabilityByTen()
    {
        IncreaseProbability(10); // 확률을 10만큼 증가
    }

    public void DecreaseProbabilityAndRefundLentz()
    {
        if (totalUsedLentz > 0)
        {
            // 확률 감소
            CurrentProbability -= 1;
            // 렌츠 반환
            LentzManager.Instance.AddLentz(1);
            totalUsedLentz -= 1; // 사용된 렌츠 양 감소

            UpdateProbabilityUI();
        }
        else
        {
            Debug.Log("No lentz to refund, or probability is at minimum.");
        }
    }

    public void DecreaseProbabilityAndRefundLentzByTen()
    {
        int refundAmount = 10; // 환불할 렌츠의 양

        // 실제로 환불할 수 있는 양 계산 (사용된 렌츠 양과 현재 확률을 고려)
        int actualRefundAmount = Mathf.Min(refundAmount, totalUsedLentz, CurrentProbability);

        if (actualRefundAmount > 0)
        {
            // 확률 감소
            CurrentProbability -= actualRefundAmount;
            // 렌츠 반환
            LentzManager.Instance.AddLentz(actualRefundAmount);
            totalUsedLentz -= actualRefundAmount; // 사용된 렌츠 양 감소

            UpdateProbabilityUI();
        }
        else
        {
            Debug.Log("No lentz to refund, or probability is at minimum.");
        }
    }
    private void UpdateProbabilityUI()
    {
        if (LensDataManager.Instance.CurrentLens != null)
        {
            LensData currentLensData = null;
            if (LensDataManager.Instance.CurrentLens.tag == "LensLeft")
            {
                currentLensData = LensDataManager.Instance.LensDataLeft;
            }
            else if (LensDataManager.Instance.CurrentLens.tag == "LensRight")
            {
                currentLensData = LensDataManager.Instance.LensDataRight;
            }

            if (currentLensData != null)
            {
                enhancerTextUIUpdater.UpdateLensInfo(currentLensData.Lightrical, CurrentProbability);
            }
        }
    }

    private float GetBaseProbability(int lightrical)
    {
        // lightrical 값에 따른 강화 확률 설정
        switch (lightrical)
        {
            case 0: return 90f; 
            case 1: return 80f; 
            case 2:
            case 3: return 70f; 
            case 4:
            case 5: return 60f; 
            case 6:
            case 7: return 50f; 
            default: return 50f; 
        }
    }


    private float GetSphericalAdjustment(float spherical)
    {
        // 0.5마다 기본 확률 2% 감소
        return Mathf.Floor(spherical / 0.5f) * 2f;
    }

    private float GetCylindricalAdjustment(float cylindrical)
    {
        // 1마다 기본 확률 0.2% 감소
        return cylindrical * 0.2f;
    }

    private float GetLentzAdjustment(int usedLentz)
    {
        // 렌츠 1개당 확률 1% 증가
        return usedLentz * 1f;
    }
}