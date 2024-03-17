using UnityEngine;
using UnityEngine.UI;

public class ScrollAnimationController : MonoBehaviour
{
    public Image animationImage;
    public Sprite[] animationFrames;

    private int currentFrameIndex = 0;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            UpdateAnimationFrame(scroll < 0 ? 1 : -1);
        }
    }

    void UpdateAnimationFrame(int direction)
    {
        currentFrameIndex += direction;

        if (currentFrameIndex >= animationFrames.Length)
        {
            currentFrameIndex = 0;
            // Update blur level in CrossController
        }
        else if (currentFrameIndex < 0)
        {
            currentFrameIndex = animationFrames.Length - 1;
        }

        animationImage.sprite = animationFrames[currentFrameIndex];
    }
}