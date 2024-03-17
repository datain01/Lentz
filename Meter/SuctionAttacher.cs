using UnityEngine;

public class SuctionAttacher : MonoBehaviour
{
    public GameObject suctionPrefab; // 석션 프리팹

    // ButtonMeterStamp 버튼 UI가 눌렸을 때 호출될 메서드
    // SuctionAttacher 스크립트 수정
public void AttachSuctionToLens()
{
    if (DragController.isLensOnTargetBase && DragController.currentLensOnTargetBase != null)
    {
        // 현재 렌즈의 Transform을 가져옵니다.
        Transform lensTransform = DragController.currentLensOnTargetBase.transform;

        // 렌즈의 자식 오브젝트 중에서 석션 프리팹과 동일한 이름을 가진 오브젝트가 있는지 확인합니다.
        bool alreadyHasSuction = false;
        foreach (Transform child in lensTransform)
        {
            if (child.CompareTag("Suction")) 
            {
                alreadyHasSuction = true;
                break;
            }
        }

        // 이미 석션이 부착되어 있지 않다면, 석션을 인스턴스화합니다.
        if (!alreadyHasSuction)
        {
            GameObject suctionInstance = Instantiate(suctionPrefab, lensTransform.position, Quaternion.identity, lensTransform);
            suctionInstance.name = suctionPrefab.name; // 인스턴스화된 석션에 이름을 설정합니다.
        }
    }
}



}
