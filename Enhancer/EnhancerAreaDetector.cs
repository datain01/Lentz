using UnityEngine;

public class EnhancerAreaDetector : MonoBehaviour
{
    public EnhancerHandle enhancerHandle; // EnhancerHandle 스크립트를 인스펙터에서 할당

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LensLeft") || other.CompareTag("LensRight")) // "Lens" 태그를 가진 오브젝트가 Enhancer 영역에 들어옴
        {
            Debug.Log("EnhancerHandle 비활성화");
            enhancerHandle.SetButtonActive(false); // EnhancerHandle 버튼을 비활성화
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("LensLeft") || other.CompareTag("LensRight")) // "Lens" 태그를 가진 오브젝트가 Enhancer 영역에서 나감
        {
            Debug.Log("EnhancerHandle 활성화");
            enhancerHandle.SetButtonActive(true); // EnhancerHandle 버튼을 활성화
        }
    }
}
