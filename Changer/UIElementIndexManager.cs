using System.Collections.Generic;
using UnityEngine;

public class UIElementIndexManager : MonoBehaviour
{
    public List<UIElementChanger> elementChangers;
    [SerializeField]
    private int currentIndex = 0;

    void Start()
    {
        // 게임 시작 시 모든 UI 요소 업데이트
        UpdateElementIndex(currentIndex);
    }

    public void UpdateElementIndex(int newIndex)
    {
        currentIndex = newIndex;
        foreach (var changer in elementChangers)
        {
            if (changer != null)
            {
                changer.UpdateElement(currentIndex);
            }
        }
    }
}
