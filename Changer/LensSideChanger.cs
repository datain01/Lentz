using UnityEngine;
using UnityEngine.UI;

public class LensSideChanger : MonoBehaviour
{
    public Sprite sideSprite; // 옆모습 이미지
    private Image lensImage; // 렌즈 이미지 컴포넌트

    // Start 메서드에서 초기 설정을 하지 않고, 복원 시 UIElementChanger의 currentSprite 사용
    void Start()
    {
        lensImage = GetComponent<Image>();
    }

    // Enhancer 영역에 드래그앤드랍했을 때 호출
    public void ChangeToSideView()
    {
        lensImage.sprite = sideSprite;
    }

    // Enhancer 영역에서 나왔을 때 호출
    public void RestoreDefaultView()
    {
        // UIElementChanger 컴포넌트에서 현재 스프라이트를 가져와서 설정
        UIElementChanger uiElementChanger = GetComponent<UIElementChanger>();
        if (uiElementChanger != null)
        {
            lensImage.sprite = uiElementChanger.currentSprite;
        }
    }
}
