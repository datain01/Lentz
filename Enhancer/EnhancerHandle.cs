using System.Collections;
using UnityEngine;

public class EnhancerHandle : MonoBehaviour
{
    public GameObject enhancer; // Enhancer 오브젝트를 인스펙터에서 할당
    private bool isOpen = false; // 현재 Enhancer가 열린 상태인지를 나타내는 플래그
    private float openPosX = 700f; // 열린 상태의 X 위치
    private float closedPosX = 1200f; // 닫힌 상태의 X 위치
    private float moveDuration = 0.3f; // Enhancer 이동에 걸리는 시간 (초)

    public void ToggleEnhancer()
    {
        StopAllCoroutines(); // 현재 진행 중인 모든 코루틴을 중지 (부드러운 이동 중복 방지)
        StartCoroutine(MoveEnhancer(isOpen ? closedPosX : openPosX)); // Enhancer 이동 코루틴 시작
        isOpen = !isOpen; // Enhancer 상태 토글
    }

    IEnumerator MoveEnhancer(float targetPosX)
    {
        float elapsedTime = 0; // 경과 시간 초기화
        Vector3 startPos = enhancer.transform.localPosition; // 시작 위치
        Vector3 targetPos = new Vector3(targetPosX, enhancer.transform.localPosition.y, enhancer.transform.localPosition.z); // 목표 위치

        while (elapsedTime < moveDuration)
        {
            enhancer.transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsedTime / moveDuration); // 부드러운 이동
            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            yield return null; // 다음 프레임까지 대기
        }

        enhancer.transform.localPosition = targetPos; // 최종 위치 확정 (보간으로 인한 작은 오차 보정)
    }
}
