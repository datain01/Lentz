using UnityEngine;
using TMPro; 

public class EnhancerTextUIUpdater : MonoBehaviour
{
    public TextMeshProUGUI textLightrical;

    // 렌즈의 Lightrical 수치를 텍스트 UI에 업데이트하는 메서드
    public void UpdateLightricalDisplay(int lightrical)
    {
        if (textLightrical != null)
        {
            textLightrical.text = "L " + lightrical.ToString();
        }
    }
}
