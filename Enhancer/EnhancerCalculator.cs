using UnityEngine;

public class EnhancerCalculator : MonoBehaviour
{

    // 강화 확률 계산 메서드
    public float CalculateEnhancementProbability(float spherical, float cylindrical, int lightrical, int usedLentz)
    {
        // 기본 확률 설정
        float baseProbability = GetBaseProbability(lightrical);
        
        // 도수와 난시에 따른 확률 조정
        baseProbability -= GetSphericalAdjustment(spherical);
        baseProbability -= GetCylindricalAdjustment(cylindrical);
        
        // 렌츠 사용에 따른 확률 증가
        baseProbability += usedLentz * 1f; // 렌츠 1개당 확률 1% 증가
        
        // 확률 100% 초과 방지
        return Mathf.Min(baseProbability, 100f);
    }

    private float GetBaseProbability(int lightrical)
    {
        switch (lightrical)
        {
            case 1: return 90f;
            case 2: return 80f;
            case 3: return 70f;
            case 4: return 60f;
            case 5: return 50f;
            // 추가 경휘 단계에 따른 확률 설정 가능
            default: return 50f; // 기본값
        }
    }

    private float GetSphericalAdjustment(float spherical)
    {
        // 0.5마다 기본 확률 2% 감소
        return Mathf.Floor(spherical / 0.5f) * 2f;
    }

    private float GetCylindricalAdjustment(float cylindrical)
    {
        // 1마다 기본 확률 0.2% 감소
        return cylindrical * 0.2f;
    }
}
