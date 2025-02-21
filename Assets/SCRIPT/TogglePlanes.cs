using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TogglePlanes : MonoBehaviour
{
    public ARPlaneManager planeManager;

    public void TogglePlaneDetection()
    {
        bool isActive = planeManager.enabled;
        planeManager.enabled = !isActive;

        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(!isActive);
        }
    }
}
