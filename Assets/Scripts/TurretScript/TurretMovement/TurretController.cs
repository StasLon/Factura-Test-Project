using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform turretPivot;    // сам объект турели (ось поворота)
    public Transform laserStart;     // точка, откуда идёт лазер
    public LineRenderer laserLine;
    public bool canMoveTurret;

    public float rotationSpeed = 5f; // скорость поворота турели
    public float laserLength = 10f;  // длина лазера

    // ограничения поворота по Y относительно начального направления
    public float minY = -60f; // влево
    public float maxY = 60f;  // вправо
    private float initialY;    // исходный угол Y

    void Start()
    {
        if (turretPivot != null)
            initialY = turretPivot.eulerAngles.y;
    }

    void Update()
    {
        if (canMoveTurret)
        {
            RotateTurret();
            UpdateLaser(); 
        }
        
    }

    void RotateTurret()
    {
        if (turretPivot == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, turretPivot.position);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 direction = hitPoint - turretPivot.position;
            direction.y = 0;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // ограничиваем угол
                float desiredY = Mathf.DeltaAngle(0, targetRotation.eulerAngles.y);
                float clampedY = Mathf.Clamp(desiredY - initialY, minY, maxY) + initialY;

                Quaternion clampedRotation = Quaternion.Euler(0, clampedY, 0);

                turretPivot.rotation = Quaternion.Lerp(turretPivot.rotation, clampedRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void UpdateLaser()
    {
        if (laserLine == null || laserStart == null) return;

        laserLine.positionCount = 2;
        laserLine.SetPosition(0, laserStart.position);

        // Конец линии = вперед от турели на laserLength
        Vector3 endPoint = laserStart.position + turretPivot.forward * laserLength;
        laserLine.SetPosition(1, endPoint);
    }
}