using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform laserStart;
    [SerializeField] private LineRenderer laser;

    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float laserLength = 10f;

    [SerializeField] private float minY = -60f;
    [SerializeField] private float maxY = 60f;

    private float _initialY;
    private bool _enabled;

    public void Enable() => _enabled = true;
    public void Disable() => _enabled = false;

    private void Awake()
    {
        if (pivot != null)
            _initialY = pivot.eulerAngles.y;
    }

    private void Update()
    {
        if (!_enabled) return;

        Rotate();
        UpdateLaser();
    }

    private void Rotate()
    {
        if (pivot == null) return;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.up, pivot.position);

        if (!plane.Raycast(ray, out float enter)) return;

        var hit = ray.GetPoint(enter);
        var dir = hit - pivot.position;
        dir.y = 0;

        if (dir.sqrMagnitude < 0.001f) return;

        var targetRot = Quaternion.LookRotation(dir);

        float desiredY = Mathf.DeltaAngle(0, targetRot.eulerAngles.y);
        float clampedY = Mathf.Clamp(desiredY - _initialY, minY, maxY) + _initialY;

        var finalRot = Quaternion.Euler(0, clampedY, 0);

        pivot.rotation = Quaternion.Lerp(
            pivot.rotation,
            finalRot,
            rotationSpeed * Time.deltaTime
        );
    }

    private void UpdateLaser()
    {
        if (laser == null || laserStart == null) return;

        laser.positionCount = 2;
        laser.SetPosition(0, laserStart.position);
        laser.SetPosition(1, laserStart.position + pivot.forward * laserLength);
    }
}