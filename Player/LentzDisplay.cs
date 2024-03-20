using UnityEngine;
using TMPro;

public class LentzDisplay : MonoBehaviour
{
    public TextMeshProUGUI lentzText; // Inspector에서 할당

    // 렌츠 양 표시 업데이트 메서드
    public void UpdateDisplay(int lentzAmount)
    {
        lentzText.text = "Lentz: " + lentzAmount.ToString();
    }
}
