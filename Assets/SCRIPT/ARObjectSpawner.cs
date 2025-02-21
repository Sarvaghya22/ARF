using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ARObjectSpawner : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    public GameObject objectPrefab;
    public Button hideButton; // UI Button to hide/show objects

    private List<ARRaycastHit> arHits = new List<ARRaycastHit>();
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private bool areObjectsHidden = false;

    void Start()
    {
        // Attach the button click event
        if (hideButton != null)
        {
            hideButton.onClick.AddListener(ToggleObjectVisibility);
        }
    }

    void Update()
    {
        if (planeManager == null || !planeManager.enabled || raycastManager == null)
            return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            if (raycastManager.Raycast(touch.position, arHits, TrackableType.PlaneWithinPolygon))
            {
                ARPlane minYPlane = GetLowestYPlane(arHits);
                if (minYPlane != null)
                {
                    Pose hitPose = arHits.First(hit => hit.trackableId == minYPlane.trackableId).pose;
                    GameObject newObject = Instantiate(objectPrefab, hitPose.position, hitPose.rotation);
                    spawnedObjects.Add(newObject);
                }
            }
        }
    }

    // Returns the ARPlane with the lowest Y position
    private ARPlane GetLowestYPlane(List<ARRaycastHit> hits)
    {
        return hits
            .Select(hit => planeManager.GetPlane(hit.trackableId))
            .Where(plane => plane != null)
            .OrderBy(plane => plane.transform.position.y)
            .FirstOrDefault();
    }

    // Toggle visibility of spawned objects
    private void ToggleObjectVisibility()
    {
        areObjectsHidden = !areObjectsHidden;

        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                obj.SetActive(!areObjectsHidden);
            }
        }

        // Change button text
        hideButton.GetComponentInChildren<Text>().text = areObjectsHidden ? "Show Objects" : "Hide Objects";
    }
}
