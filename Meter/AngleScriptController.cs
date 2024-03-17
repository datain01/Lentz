using UnityEngine;

public class AngleScriptController : MonoBehaviour
{
    public Transform meterCircleTransform;
    public float activationAngleThreshold = 1f;
    public CrossController crossController;
    public ScrollAnimationController scrollAnimationController;

    void Update()
    {
        bool isAngleActive = Mathf.Abs(meterCircleTransform.localEulerAngles.z) < activationAngleThreshold;
        bool isLensOnTargetBase = LensDataManager.Instance.CurrentLens != null;

        crossController.enabled = isAngleActive && isLensOnTargetBase;
        scrollAnimationController.enabled = isAngleActive;
        crossController.UpdateCrossBlurImageVisibility(isAngleActive && isLensOnTargetBase);
    }
}
