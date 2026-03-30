using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Trails : MonoBehaviour
{
    public Transform target; // машина
    public float pointDistance = 0.5f; // дистанци€ между точками
    public float lifeTime = 3f; // сколько живЄт след

    private LineRenderer line;
    private List<Vector3> points = new List<Vector3>();
    private List<float> times = new List<float>();

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
    }

    void Update()
    {
        if (target == null) return;

        AddPoint();
        RemoveOldPoints();
        UpdateLine();
    }

    void AddPoint()
    {
        if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], target.position) > pointDistance)
        {
            points.Add(target.position);
            times.Add(Time.time);
        }
    }

    void RemoveOldPoints()
    {
        while (points.Count > 0 && Time.time - times[0] > lifeTime)
        {
            points.RemoveAt(0);
            times.RemoveAt(0);
        }
    }

    void UpdateLine()
    {
        line.positionCount = points.Count;

        for (int i = 0; i < points.Count; i++)
        {
            line.SetPosition(i, points[i]);
        }
    }
}