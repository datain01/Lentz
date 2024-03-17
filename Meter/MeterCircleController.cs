using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MeterCircleController : MonoBehaviour
{
    public RectTransform targetToRotate; 
    private float duration = 0.3f;
    private int clickCount = 0; // 클릭 카운트 변수 추가

    public void OnButtonClick()
    {
        clickCount = (clickCount + 1) % 3; // 클릭 카운트를 1-3 사이로 반복
        if (clickCount == 0) clickCount = 3; // 0이 되는 것을 방지

        float targetZRotation = 0f;

        // 클릭 카운트에 따른 각도 조정
        switch (clickCount)
        {
            case 1:
                targetZRotation = 0f; // 첫 번째 클릭
                break;
            case 2:
                targetZRotation = 270f; // 두 번째 클릭
                break;

            default:
                targetZRotation = 180f; // 기본값
                break;
        }

        if (targetToRotate != null)
        {
            StartCoroutine(RotateToAngle(targetZRotation, duration));
        }
    }

    IEnumerator RotateToAngle(float targetZRotation, float duration)
    {
        Quaternion startRotation = targetToRotate.rotation;
        Quaternion endRotation = Quaternion.Euler(0, 0, targetZRotation);
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            targetToRotate.rotation = Quaternion.Lerp(startRotation, endRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        targetToRotate.rotation = endRotation; // 목표 회전값에 정확히 맞춤
    }
}
