using UnityEngine;
using UnityEngine.UI;

public class ScrollAnimationController : MonoBehaviour
{
    public Image animationImage;
    public Sprite[] animationFrames;
    public CrossController crossController; // CrossController에 대한 참조

    private int currentFrameIndex = 0;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0 && !crossController.IsBlurLevelMinimum())
        {
            UpdateAnimationFrame(scroll < 0 ? 1 : -1);
            crossController.AdjustBlurLevelByScroll(scroll < 0 ? 1 : -1);
        }
    }


    void UpdateAnimationFrame(int direction)
    {
        currentFrameIndex += direction;

        if (currentFrameIndex >= animationFrames.Length)
        {
            currentFrameIndex = 0;
        }
        else if (currentFrameIndex < 0)
        {
            currentFrameIndex = animationFrames.Length - 1;
        }

        animationImage.sprite = animationFrames[currentFrameIndex];
    }
}
