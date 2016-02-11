﻿using System;
using UnityEngine;

public enum BezierControlPointMode { Free, Aligned, Mirrored }

public class BezierSpline : MonoBehaviour
{
    [SerializeField]
    Vector3[] points;

    [SerializeField]
    BezierControlPointMode[] modes = { BezierControlPointMode.Mirrored, BezierControlPointMode.Aligned, BezierControlPointMode.Free };

    public int ControlPointCount
    {
        get { return points.Length; }
    }

    public int CurveCount
    {
        get { return (points.Length - 1) / 3; }
    }

    public Vector3 GetControlPoint (int i) { return points[i];}

    public void SetControlPoint (int i, Vector3 point) { points[i] = point; EnforceMode(i); }

    public BezierControlPointMode GetControlPointMode(int i) { return modes[(i + 1) / 3]; }

    public void SetControlPointMode(int i, BezierControlPointMode mode) { modes[(i + 1) / 3] = mode; EnforceMode(i); }

    public Vector3 GetPoint(float t)
    {
        int i;
        if(t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
    }

    public Vector3 GetVelocity(float t, bool normalized = false)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        Vector3 velocity = transform.TransformPoint(Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t) - transform.position);
        if (normalized)
            velocity = velocity.normalized;
        return velocity;
    }

    public void AddCurve ()
    {
        Vector3 point = points[points.Length - 1];
        Array.Resize(ref points, points.Length + 3);
        point.x += 1f;
        points[points.Length - 3] = point;
        point.x += 1f;
        points[points.Length - 2] = point;
        point.x += 1f;
        points[points.Length - 1] = point;
        Array.Resize(ref modes, modes.Length + 1);
        modes[modes.Length - 1] = modes[modes.Length - 2];
    }

    void EnforceMode (int i)
    {
        int modeIndex = (i + 1) / 3;
        BezierControlPointMode mode = modes[modeIndex];
        if (mode == BezierControlPointMode.Free || modeIndex == 0 || modeIndex == modes.Length - 1)
            return;
    }

    public void Reset()
    {
        points = new Vector3[] { new Vector3(1f, 0f, 0f), new Vector3(2f, 0f, 0f), new Vector3(3f, 0f, 0f), new Vector3(4f, 0f, 0f) };
        modes = new BezierControlPointMode[] { BezierControlPointMode.Free, BezierControlPointMode.Free };
    }
}
