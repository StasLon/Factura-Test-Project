using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float forwardSpeed = 10f;   // скорость вперёд
    public float sideAmplitude = 2f;   // насколько сильно уводит в стороны
    public float sideFrequency = 1f;   // как часто меняет направление
    public bool canMove;

    private float startX;

    void Start()
    {
        startX = transform.position.x;
    }

    void Update()
    {
        if (canMove)
        {
            MoveCar();
        }
     }

   private void MoveCar()
    {
        // движение вперёд
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // плавное движение влево-вправо
        float xOffset = Mathf.Sin(Time.time * sideFrequency) * sideAmplitude;

        Vector3 pos = transform.position;
        pos.x = startX + xOffset;

        transform.position = pos;
    }
}