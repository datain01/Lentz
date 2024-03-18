using System.Collections.Generic; // 이 줄을 추가합니다.
using UnityEngine;
using UnityEngine.UI;

public class UIElementChanger : MonoBehaviour
{
    [SerializeField]
    private List<UIElementData> elementDatas; // UI 요소 데이터 리스트

    private Image imageComponent; // 현재 UI 요소의 Image 컴포넌트
    public Sprite currentSprite;

    void Awake()
    {
        imageComponent = GetComponent<Image>();
    }

    public void UpdateElement(int index)
    {
        if (index >= 0 && index < elementDatas.Count && imageComponent != null)
        {
            var selectedData = elementDatas[index];
            imageComponent.sprite = selectedData.sprite;
            currentSprite = selectedData.sprite;
            RectTransform rectTransform = imageComponent.rectTransform;
            rectTransform.anchoredPosition = selectedData.position;
            rectTransform.sizeDelta = selectedData.size;
        }
    }
}
