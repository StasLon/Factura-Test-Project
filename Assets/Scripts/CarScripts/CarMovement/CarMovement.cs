using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float sideAmplitude = 2f;
    [SerializeField] private float sideFrequency = 1f;

    private float _startX;
    public bool _isMoving;

    public void Enable() => _isMoving = true;
    public void Disable() => _isMoving = false;

    private void Awake()
    {
        _startX = transform.position.x;
    }

    private void Update()
    {
        if (!_isMoving) return;

        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        float offset = Mathf.Sin(Time.time * sideFrequency) * sideAmplitude;

        var pos = transform.position;
        pos.x = _startX + offset;
        transform.position = pos;
    }
}