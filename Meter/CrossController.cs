using UnityEngine;
using UnityEngine.UI;

public class CrossController : MonoBehaviour
{
    public static CrossController Instance;

    public Image crossBlurImage;
    public Sprite[] blurLevels;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // 게임 시작 시 CrossBlur 이미지를 비활성화
        crossBlurImage.enabled = false;
        Debug.Log("Cross Blur 이미지 비활성화");
    }


    public void UpdateCrossBlurLevel(int blurLevel)
    {
        int imageIndex = Mathf.Clamp(blurLevel / 2, 0, blurLevels.Length - 1);
        crossBlurImage.sprite = blurLevels[imageIndex];
        UpdateCrossBlurImageVisibility(blurLevel > 0);
    }

    public void UpdateCrossBlurImageVisibility(bool isVisible)
    {
        // Image 컴포넌트의 활성화/비활성화를 제어
        crossBlurImage.enabled = isVisible;
    }

}