using UnityEngine;
using UnityEngine.UI;

public class GlassesController : MonoBehaviour
{
    public Image glassesImage;

    private bool isLensLeftPlaced = false;
    private bool isLensRightPlaced = false;

    public void SetLensPlaced(string lensTag)
{
    Debug.Log($"SetLensPlaced called with: {lensTag}");

    if (lensTag == "LensLeft")
    {
        isLensLeftPlaced = true;
        Debug.Log("LensLeft placed.");
    }
    else if (lensTag == "LensRight")
    {
        isLensRightPlaced = true;
        Debug.Log("LensRight placed.");
    }

    CheckGlassesActivation();
}

private void CheckGlassesActivation()
{
    Debug.Log($"Checking Glasses Activation: Left = {isLensLeftPlaced}, Right = {isLensRightPlaced}");

    if (isLensLeftPlaced && isLensRightPlaced)
    {
        glassesImage.raycastTarget = true;
        Debug.Log("Glasses raycastTarget is enabled.");
    }
}

}
