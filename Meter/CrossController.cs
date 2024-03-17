using UnityEngine;
using UnityEngine.UI;

public class CrossController : MonoBehaviour
{
    public static CrossController Instance;
    public Image crossBlurImage;
    public Sprite[] CrossLevels;
    private int currentBlurLevel; // 현재 블러 레벨을 추적하는 변수

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        crossBlurImage.enabled = false;
    }

    public void UpdateCrossBlurLevel(int blurLevel)
    {
        currentBlurLevel = blurLevel; // 현재 블러 레벨을 업데이트
        int imageIndex = Mathf.Clamp(currentBlurLevel / 2, 0, CrossLevels.Length - 1);
        crossBlurImage.sprite = CrossLevels[imageIndex];
        crossBlurImage.enabled = currentBlurLevel > 0;
    }

    // 스크롤 방향에 따른 블러 레벨 조정
    public void AdjustBlurLevelByScroll(int direction)
    {
        int newBlurLevel = Mathf.Clamp(currentBlurLevel - direction, 0, CrossLevels.Length - 1);

        // 최대 선명도에 도달했는지 체크
        if (newBlurLevel == 0 && currentBlurLevel != 0)
        {
            if (LensDataManager.Instance.CurrentLens != null)
            {
                LensDataManager.Instance.SetLensChecked(LensDataManager.Instance.CurrentLens.tag);
            }
        }

        currentBlurLevel = newBlurLevel;
        UpdateCrossBlurLevel(currentBlurLevel);
    }


    public void UpdateCrossBlurImageVisibility(bool isVisible)
    {
        crossBlurImage.enabled = isVisible;
    }

    public bool IsBlurLevelMinimum()
    {
        return currentBlurLevel <= 0;
    }


}
