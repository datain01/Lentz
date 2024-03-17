using UnityEngine;

public class LensDataManager : MonoBehaviour
{
    public static LensDataManager Instance;

    public LensData LensDataLeft { get; private set; }
    public LensData LensDataRight { get; private set; }

    public GameObject CurrentLens { get; set; } // 현재 TargetBase에 있는 렌즈 오브젝트

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        InitializeLensData();
    }

    private void InitializeLensData()
    {
        LensDataLeft = new LensData(Random.Range(0f, 14f), Random.Range(1f, 12f), Random.Range(1, 8));
        LensDataRight = new LensData(LensDataLeft.Spherical + Random.Range(-0.5f, 0.5f),
                                     LensDataLeft.Cylindrical + Random.Range(-0.5f, 0.5f),
                                     LensDataLeft.Lightrical);

        // Debug log for lens data
        Debug.Log($"LensLeft - Spherical: {LensDataLeft.Spherical}, Cylindrical: {LensDataLeft.Cylindrical}, Lightrical: {LensDataLeft.Lightrical}");
        Debug.Log($"LensRight - Spherical: {LensDataRight.Spherical}, Cylindrical: {LensDataRight.Cylindrical}, Lightrical: {LensDataRight.Lightrical}");
    }

    public void UpdateCurrentLens(GameObject lens)
    {
        CurrentLens = lens;
        UpdateCrossBlurLevel();

        // Debug log for the current lens on TargetBase
        Debug.Log($"Current Lens on TargetBase: {CurrentLens?.name}");
    }

    private void UpdateCrossBlurLevel()
    {
        int blurLevel = 0;
        if (CurrentLens != null)
        {
            if (CurrentLens.tag == "LensLeft")
            {
                blurLevel = LensDataLeft.CalculateBlurLevel();
            }
            else if (CurrentLens.tag == "LensRight")
            {
                blurLevel = LensDataRight.CalculateBlurLevel();
            }

            // Debug log for the current blur level
            Debug.Log($"Current Blur Level: {blurLevel}");
        }
        Debug.Log($"Cross Blur 이미지 비활성화");
        CrossController.Instance.UpdateCrossBlurLevel(blurLevel);
    }

    public void SetLensChecked(string lensTag)
    {
        if (lensTag == "LensLeft")
        {
            LensDataLeft.IsMeterChecked = true;
            Debug.Log("LensLeft가 미터로 체크되었습니다.");
        }
        else if (lensTag == "LensRight")
        {
            LensDataRight.IsMeterChecked = true;
            Debug.Log("LensRight가 미터로 체크되었습니다.");
        }
    }

    public bool GetIsLensMetered(string lensTag)
{
    if (lensTag == "LensLeft")
    {
        return LensDataLeft.IsMeterChecked;
    }
    else if (lensTag == "LensRight")
    {
        return LensDataRight.IsMeterChecked;
    }
    return false;
}



}

[System.Serializable]
public class LensData
{
    public float Spherical { get; private set; }
    public float Cylindrical { get; private set; }
    public int Lightrical { get; private set; }
    public bool IsMeterChecked { get; set; } = false;

    private float blurFactorSpherical = 0.6f;
    private float blurFactorCylindrical = 0.1f;

    public LensData(float spherical, float cylindrical, int lightrical)
    {
        Spherical = spherical;
        Cylindrical = cylindrical;
        Lightrical = lightrical;
    }

    public int CalculateBlurLevel()
    {
        float baseLevel = Mathf.Abs(Spherical) * blurFactorSpherical + Mathf.Abs(Cylindrical) * blurFactorCylindrical;
        return Mathf.Clamp(Mathf.RoundToInt(baseLevel), 0, 10);
    }
}

