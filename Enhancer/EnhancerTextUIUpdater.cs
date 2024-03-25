using UnityEngine;
using TMPro;

public class EnhancerTextUIUpdater : MonoBehaviour
{
    public TextMeshProUGUI textInfo; // Lightrical과 확률을 표시할 통합된 TextMeshProUGUI

    // 렌즈의 Lightrical 수치와 강화 확률을 텍스트 UI에 업데이트하는 메서드
    public void UpdateLensInfo(int lightrical, int probability)
    {
        if (textInfo != null)
        {
            textInfo.text = $"L {lightrical} ({probability}%)";
        }
    }
}
