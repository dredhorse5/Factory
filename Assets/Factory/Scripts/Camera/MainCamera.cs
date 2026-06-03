using UnityEngine;
using VContainer;

public class MainCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Camera targetCamera;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private AnimationCurve moveSpeedByZoom;
    

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 150f;

    [Header("Zoom")]
    [SerializeField] private float minZoom = 10f;
    [SerializeField] private float maxZoom = 100f;
    [SerializeField] private float zoomSpeed = 15f;
    [SerializeField] private float zoomSmoothTime = 0.15f;
    [SerializeField] private AnimationCurve zoomPowerCurve;

    [Header("Pitch")]
    [SerializeField] private AnimationCurve pitchCurve = AnimationCurve.Linear(0f, 45f, 1f, 85f);

    private float targetZoom;
    private float currentZoom;
    private float zoomVelocity;

    private float yaw;

    [Inject] 
    public Map map;

    private void Awake()
    {
        currentZoom = targetZoom = -targetCamera.transform.localPosition.z;
        yaw = transform.eulerAngles.y;

        UpdateCameraInstant();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();

        currentZoom = Mathf.SmoothDamp(currentZoom, targetZoom, ref zoomVelocity, zoomSmoothTime);

        UpdateCameraTransform();
    }

    private void HandleMovement()
    {
        Vector2 input = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) input.y += 1;
        if (Input.GetKey(KeyCode.S)) input.y -= 1;
        if (Input.GetKey(KeyCode.D)) input.x += 1;
        if (Input.GetKey(KeyCode.A)) input.x -= 1;

        Vector3 move = transform.forward * input.y + transform.right * input.x;

        move.y = 0f;

        transform.position += move.normalized * moveSpeed * moveSpeedByZoom.Evaluate(currentZoom) * Time.deltaTime;
    }

    private void HandleRotation()
    {
        if (!Input.GetMouseButton(1))
            return;

        yaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

    private void HandleZoom(bool instant = false)
    {
        float wheel = Input.mouseScrollDelta.y;

        if (!instant && Mathf.Abs(wheel) < 0.01f)
            return;

        targetZoom -= wheel * zoomSpeed * zoomPowerCurve.Evaluate(currentZoom);

        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    }

    private void UpdateCameraTransform()
    {
        float t = Mathf.InverseLerp(
            minZoom,
            maxZoom,
            currentZoom);

        float pitch = pitchCurve.Evaluate(t);

        cameraPivot.localRotation =
            Quaternion.Euler(pitch, 0f, 0f);

        targetCamera.transform.localPosition =
            new Vector3(0f, 0f, -currentZoom);
    }

    private void UpdateCameraInstant()
    {
        float t = Mathf.InverseLerp(minZoom, maxZoom, currentZoom);

        float pitch = pitchCurve.Evaluate(t);

        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        targetCamera.transform.localPosition = new Vector3(0f, 0f, -currentZoom);
        HandleZoom(true);
    }

    public Vector2Int GetLookAtCell()
    {
        return map.GetCellByPosition(transform.position);
    }
}