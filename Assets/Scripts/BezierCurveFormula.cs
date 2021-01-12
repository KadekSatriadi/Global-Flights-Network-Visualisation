using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BezierCurveFormula for Unity
/// </summary>
public static class BezierCurveFormula
{
    public static List<Vector3> GetLinear(Vector3 p0, Vector3 p1, float t, int nsegments)
    {
        List<Vector3> points = new List<Vector3>();
        float inc = 1f/nsegments;
        float i = 0f;
        while(i < t)
        {
            Vector3 pt = p0 + i * (p1 - p0);
            points.Add(pt);
            i += inc;
        }

        return points;
    }

    public static List<Vector3> GetQuadratic(Vector3 p0, Vector3 p1, Vector3 p2, float t, int nsegments)
    {
        List<Vector3> points = new List<Vector3>();
        float inc = 1f / nsegments;
        float i = 0f;
        while (i <= t)
        {
            Vector3 pt = p1 + (1f - i) * (1f - i) * (p0 - p1) + i * i * (p2 - p1);
            points.Add(pt);
            i += inc;
        }

        return points;
    }

    public static List<Vector3> GetCubic(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t, int nsegments)
    {
        List<Vector3> points = new List<Vector3>();
        float inc = 1f / nsegments;
        float i = 0f;
        while (i <= t)
        {
            Vector3 pt = Mathf.Pow((1f - i), 3)  * p0 + 3f * (1f-i) * (1f-i) * i * p1 + 3f * (1f-i) * i * i * p2 + i * i * i * p3;
            points.Add(pt);
            i += inc;
        }

        return points;
    }
}