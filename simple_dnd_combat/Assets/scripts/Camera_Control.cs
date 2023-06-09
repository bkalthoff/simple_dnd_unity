using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    private Vector3 mouseOrigin;
    [SerializeField]
    private new Camera camera;
    private Unit_Manager unitManager;
    private bool lockzoom = false;

    void Start()
    {
        unitManager = FindObjectOfType<Unit_Manager>();
        if (unitManager != null)
        {
            unitManager.OnHoverUnitChanged += HandleHoverUnitChanged;
        }
        else
        {
            Debug.LogWarning("Unit_Manager script not found in the scene!");
        }
    }

    void OnDestroy()
    {
        if (unitManager != null)
        {
            unitManager.OnHoverUnitChanged -= HandleHoverUnitChanged;
        }
    }

    void HandleHoverUnitChanged(GameObject unit)
    {
        // Handle the hover unit change here
        if (unit != null)
        {
            lockzoom = true;
            Debug.Log("Camera: Hovered unit - " + unit.name);
        }
        else
        {
            lockzoom = false;
            Debug.Log("Camera: No unit is being hovered.");
        }
    }

    void Update()
    {
        PanCamera();
        ZoomCamera();
    }

    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(2))
        {
            mouseOrigin = camera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 diff = mouseOrigin - camera.ScreenToWorldPoint(Input.mousePosition);
            camera.transform.position += diff;
        }
    }

    private void ZoomCamera()
    {
        if (lockzoom)
        {
            return;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            camera.orthographicSize /= 1.1f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            camera.orthographicSize *= 1.1f;
        }
        if (camera.orthographicSize < 1)
        {
            camera.orthographicSize = 1;
        }
        if (camera.orthographicSize > 100)
        {
            camera.orthographicSize = 100;
        }
    }
}