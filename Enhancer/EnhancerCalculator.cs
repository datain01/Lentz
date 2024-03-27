using UnityEngine;
using UnityEngine.UI;


public class EnhancerCalculator : MonoBehaviour
{
    public static EnhancerCalculator Instance { get; private set; }
    public EnhancerAreaDetector enhancerAreaDetector;
    public EnhancerTextUIUpdater enhancerTextUIUpdater;

    public int CurrentProbability { get; set; }
    private bool hasCalculatedProbability = false;
    public int pendingLentzUsage = 0;
    // public int pendingLentzUsage = 0;

    [SerializeField]
    private Button[] enhanceButtons; // Unity Inspector에서 할당



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

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

    public void ResetPendingLentzUsage()
    {
        pendingLentzUsage = 0;
        // 필요한 경우 여기에서 추가적인 UI 업데이트 로직을 실행
    }

    public void PrepareLentzUsage(int amount)
    {
        if (LentzManager.Instance.lentzAmount >= (pendingLentzUsage + amount))
        {
            pendingLentzUsage += amount;
        }
        else
        {
            Debug.Log("Not enough Lentz for this operation.");
        }
    }

        public void TryEnhanceLightrical()
        {
            if (!enhancerAreaDetector.IsLensAttached() || pendingLentzUsage > LentzManager.Instance.lentzAmount) return;

            // 렌츠 사용을 진행합니다.
            LentzManager.Instance.UseLentz(pendingLentzUsage); // 실제로 렌츠를 사용합니다.
            pendingLentzUsage = 0; // 사용 후 초기화

            LensData currentLensData = GetCurrentLensData();
            if (currentLensData == null) return;

            int randomChance = Random.Range(0, 100);
            if (randomChance < CurrentProbability)
            {
                // 강화 성공
                Debug.Log("Enhancement successful!");
                currentLensData.IncreaseLightrical();
            }
            else
            {
                Debug.Log("Enhancement failed.");
            }

            pendingLentzUsage = 0;
            CurrentProbability = CalculateEnhancementProbability(currentLensData.Spherical, currentLensData.Cylindrical, currentLensData.Lightrical, pendingLentzUsage);
            enhancerTextUIUpdater.UpdateLensInfo(currentLensData.Lightrical, CurrentProbability);

            if (currentLensData.Lightrical >= 8)
            {
                foreach (var button in enhanceButtons){
                button.interactable = false; // 경휘가 8이상이면 버튼 비활성화
                } 
                Debug.Log("Lightrical reached maximum value. Enhancement disabled.");
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
        int potentialNewUsage = pendingLentzUsage + increaseAmount;
        // 렌츠가 충분한지 확인하고, 충분하다면 pendingLentzUsage를 업데이트합니다.
        if (LentzManager.Instance.lentzAmount >= potentialNewUsage)
        {
            // 여기서 확률을 재계산하고 UI를 업데이트합니다.
            pendingLentzUsage = potentialNewUsage;
            // 임시 확률 업데이트 (실제 사용 전 확률 변경 반영)
            LensData currentLensData = GetCurrentLensData();
            if (currentLensData != null)
            {
                int tempProbability = CalculateEnhancementProbability(currentLensData.Spherical, currentLensData.Cylindrical, currentLensData.Lightrical, pendingLentzUsage);
                enhancerTextUIUpdater.UpdateLensInfo(currentLensData.Lightrical, tempProbability);
            }
        }
        else
        {
            Debug.Log("Not enough Lentz to prepare for this probability increase.");
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
    if (pendingLentzUsage > 0)
    {
        // 사용할 예정인 렌츠 감소
        pendingLentzUsage -= 1;

        // 임시 확률을 업데이트합니다.
        UpdateTemporaryProbability();
    }
    else
    {
        Debug.Log("No pending lentz to decrease.");
    }
}

public void DecreaseProbabilityAndRefundLentzByTen()
{
    int actualDecreaseAmount = Mathf.Min(10, pendingLentzUsage);
    if (actualDecreaseAmount > 0)
    {
        // 사용할 예정인 렌츠 감소
        pendingLentzUsage -= actualDecreaseAmount;

        // 임시 확률을 업데이트합니다.
        UpdateTemporaryProbability();
    }
    else
    {
        Debug.Log("No pending lentz to decrease.");
    }
}


    // 확률을 업데이트하고 UI를 갱신하는 새 메서드입니다.
    private void UpdateTemporaryProbability()
    {
        LensData currentLensData = GetCurrentLensData();
        if (currentLensData != null)
        {
            int tempProbability = CalculateEnhancementProbability(currentLensData.Spherical, currentLensData.Cylindrical, currentLensData.Lightrical, pendingLentzUsage);
            enhancerTextUIUpdater.UpdateLensInfo(currentLensData.Lightrical, tempProbability);
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
