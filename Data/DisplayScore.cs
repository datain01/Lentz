using UnityEngine;
using UnityEngine.UI; // UI 관련 컴포넌트를 사용하기 위해 필요합니다.
using TMPro; // TextMeshPro 사용시 필요합니다.

public class DisplayScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Inspector에서 할당

    private void Start()
    {
        if (ScoreManager.Instance != null)
        {
            // ScoreManager의 싱글톤 인스턴스로부터 스코어를 가져와 텍스트에 표시합니다.
            scoreText.text = "Score: " + ScoreManager.Instance.Score.ToString();
        }
        else
        {
            // ScoreManager가 존재하지 않는 경우 (예외처리)
            scoreText.text = "Score: N/A";
        }
    }
}
