using UnityEngine;

public class EnhancerCalculator : MonoBehaviour
{
    public EnhancerAreaDetector enhancerAreaDetector;
    public EnhancerTextUIUpdater enhancerTextUIUpdater;

    public int CurrentProbability { get; set; }
    private bool hasCalculatedProbability = false;
    public int totalUsedLentz = 0;


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


    void ApplyEnhancement()
    {
        // 렌즈 데이터 매니저에서 현재 렌즈 데이터를 가져옴
        LensDataManager lensDataManager = LensDataManager.Instance;
        if (lensDataManager != null && lensDataManager.CurrentLens != null)
        {
            LensData lensData;
            if (lensDataManager.CurrentLens.tag == "LensLeft")
            {
                lensData = lensDataManager.LensDataLeft;
            }
            else // if (lensDataManager.CurrentLens.tag == "LensRight")
            {
                lensData = lensDataManager.LensDataRight;
            }

            // 렌즈 데이터 로그 출력
            Debug.Log($"Lens Data - Spherical: {lensData.Spherical}, Cylindrical: {lensData.Cylindrical}, Lightrical: {lensData.Lightrical}");

            // 강화 확률 계산
            CurrentProbability = CalculateEnhancementProbability(lensData.Spherical, lensData.Cylindrical, lensData.Lightrical, 0);
        
            enhancerTextUIUpdater.UpdateLensInfo(lensData.Lightrical, CurrentProbability);
            // 강화 확률 로그 출력
            Debug.Log($"Enhancement Probability: {CurrentProbability}%");
        }
    }


    // 강화 확률 계산 메서드
    public int CalculateEnhancementProbability(float spherical, float cylindrical, int lightrical, int usedLentz)
    {
        // 기본 확률 설정
        float baseProbability = GetBaseProbability(lightrical);
        
        // 도수와 난시에 따른 확률 조정
        baseProbability -= GetSphericalAdjustment(spherical);
        baseProbability -= GetCylindricalAdjustment(cylindrical);
        
        // 렌츠 사용에 따른 확률 증가
        baseProbability += GetLentzAdjustment(usedLentz);
        
        // 확률 100% 초과 방지 및 반올림 처리
        int finalProbability = Mathf.Clamp(Mathf.CeilToInt(baseProbability), 0, 100);
        return finalProbability;
    }

    public void IncreaseProbability(int increaseAmount)
    {
        // 실제 증가시킬 수 있는 최대 확률 계산
        int actualIncreaseAmount = Mathf.Min(increaseAmount, 100 - CurrentProbability);

        // 실제 증가시킬 수 있는 확률이 0보다 클 때만 작업 수행
        if (actualIncreaseAmount > 0)
        {
            // 렌츠가 충분한지 확인
            if (LentzManager.Instance.lentzAmount >= actualIncreaseAmount)
            {
                // 필요한 만큼의 Lentz 사용
                LentzManager.Instance.UseLentz(actualIncreaseAmount);

                // 확률 증가
                CurrentProbability += actualIncreaseAmount;
                // 확률 100% 초과 방지는 이제 필요하지 않음(이미 계산에서 반영됨)

                // 확률 변경을 UI에 반영
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
