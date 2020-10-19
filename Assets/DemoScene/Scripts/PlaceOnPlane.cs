using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;
    [SerializeField]
    GameObject m_PlacedPrefabDummy;

    public ARSessionOrigin ARSessionOrigin;
    public ARPlaneManager aRPlaneManager;
    private bool anchorSet;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 1)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    public void PlaceAnchor()
    {
        m_PlacedPrefab.transform.position = m_PlacedPrefabDummy.transform.position;
        m_PlacedPrefab.transform.rotation = m_PlacedPrefabDummy.transform.rotation;
        anchorSet = true;
    }

    void Update()
    {
        if (Camera.main == null)
            return;

        if (m_RaycastManager.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;

            ARPlane arp = aRPlaneManager.GetPlane(s_Hits[0].trackableId);

            if (arp.classification.ToString() == "Floor")
            {
                m_PlacedPrefabDummy.transform.position = hitPose.position;
                m_PlacedPrefabDummy.transform.rotation = hitPose.rotation;
                m_PlacedPrefabDummy.transform.LookAt(ARSessionOrigin.camera.transform.position);
                m_PlacedPrefabDummy.transform.eulerAngles = new Vector3(0, m_PlacedPrefabDummy.transform.eulerAngles.y + 180, 0);
            }
        }
    }

    public bool isAnchorSet()
    {
        return anchorSet;
    }

static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
ARRaycastManager m_RaycastManager;
}