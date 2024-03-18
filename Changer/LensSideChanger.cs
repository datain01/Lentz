using UnityEngine;
using UnityEngine.UI;

public class LensSideChanger : MonoBehaviour
{
    public Sprite sideSprite; // 옆모습 이미지
    private Image lensImage; // 렌즈 이미지 컴포넌트
    private Sprite defaultSprite; // 기본 이미지는 동적으로 설정

    void Start()
    {
        lensImage = GetComponent<Image>();
        // Start가 호출될 때 Image 컴포넌트의 현재 스프라이트를 기본 이미지로 설정
        defaultSprite = lensImage.sprite;
    }

    // Enhancer 영역에 드래그앤드랍했을 때 호출
    public void ChangeToSideView()
    {
        lensImage.sprite = sideSprite;
    }

    // Enhancer 영역에서 나왔을 때 호출
    public void RestoreDefaultView()
    {
        lensImage.sprite = defaultSprite;
    }
}
