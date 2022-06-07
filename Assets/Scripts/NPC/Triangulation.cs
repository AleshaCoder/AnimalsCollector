﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Triangulation
{
    [Serializable]
    public enum LinesRelationship
    {
        Intersect,
        Parallel,
        Superposition,
        Equal,
        Non
    }
    public struct Line
    {
        public Vector3 Start;
        public Vector3 End;

        public Line(Vector3 start, Vector3 end)
        {
            Start = start;
            End = end;
        }
    }

    public static bool HasIntersectionForSegments(Line line1, Line line2)
    {
        if (GetLinesRelationship(line1.Start, line1.End, line2.Start, line2.End, out Vector3 intersectionPoint, true, false, true) == LinesRelationship.Intersect)
        {
            if (HasPointInSegment(line1, intersectionPoint) && HasPointInSegment(line2, intersectionPoint))
                return true;
        }        
        return false;
    }

    public static LinesRelationship GetLinesRelationship(Vector3 a0, Vector3 a1, Vector3 b0, Vector3 b1, out Vector3 intersectionPoint,
        bool x = true, bool y = true, bool z = true)
    {
        if (!x)
            a0.x = a1.x = b0.x = b1.x = 0;
        if (!y)
            a0.y = a1.y = b0.y = b1.y = 0;
        if (!z)
            a0.z = a1.z = b0.z = b1.z = 0;

        var uxy = (b0.x - a0.x - (b0.y - a0.y) / (a1.y - a0.y) * (a1.x - a0.x)) / ((b1.y - b0.y) / (a1.y - a0.y) * (a1.x - a0.x) - (b1.x - b0.x));
        var uxz = (b0.x - a0.x - (b0.z - a0.z) / (a1.z - a0.z) * (a1.x - a0.x)) / ((b1.z - b0.z) / (a1.z - a0.z) * (a1.x - a0.x) - (b1.x - b0.x));
        var uyz = (b0.y - a0.y - (b0.z - a0.z) / (a1.z - a0.z) * (a1.y - a0.y)) / ((b1.z - b0.z) / (a1.z - a0.z) * (a1.y - a0.y) - (b1.y - b0.y));

        var u = !float.IsNaN(uxy) ? uxy : (!float.IsNaN(uxz) ? uxz : uyz);


        var tx = (b0.x + u * (b1.x - b0.x) - a0.x) / (a1.x - a0.x);
        var ty = (b0.y + u * (b1.y - b0.y) - a0.y) / (a1.y - a0.y);
        var tz = (b0.z + u * (b1.z - b0.z) - a0.z) / (a1.z - a0.z);

        var t = !float.IsNaN(tx) ? tx : (!float.IsNaN(ty) ? ty : tz);

        intersectionPoint = (a1 - a0) * t + a0;

        List<LinesRelationship> list = new List<LinesRelationship>();

        if (a0 == b0 && a1 == b1)
            return LinesRelationship.Intersect;
        if (a0 == b1 && a1 == b0 || a0 == b0 && a1 == b1)
            return LinesRelationship.Intersect;
        if ((a0 - a1).normalized == (b0 - b1).normalized || (a0 - a1).normalized == (b1 - b0).normalized)
            return LinesRelationship.Parallel;
        if (!(float.IsNaN(intersectionPoint.x) && a0.x - a1.x != 0 && b0.x - b1.x != 0 || float.IsNaN(intersectionPoint.y) && a0.y - a1.y != 0 && b0.y - b1.y != 0 || float.IsNaN(intersectionPoint.z) && a0.z - a1.z != 0 && b0.z - b1.z != 0))
            return LinesRelationship.Intersect;
        return LinesRelationship.Non;
    }

    private static bool HasPointInSegment(Line line, Vector3 point)
    {
        float distance1 = Vector3.Distance(line.Start, point);
        float distance2 = Vector3.Distance(line.End, point);
        float lineDistance = Vector3.Distance(line.Start, line.End);
        if (Mathf.Abs(distance1 + distance2 - lineDistance) <= 0.1f)
            return true;
        return false;
    }

    public static void GetResult(List<Vector3> points, bool clockwise, Vector3 upAxis, out Vector3[] verticles, out int[] triangles, out Vector2[] uvcoords)
    {
        var qto = Quaternion.FromToRotation(Vector3.up, upAxis);
        var qfrom = Quaternion.FromToRotation(upAxis, Vector3.up);
        //Triangulation
        var ps = points.Select(v => qto * v).ToArray();
        var y = ps.Select(v => v.y).Sum() / ps.Length;

        var pto = ps.Select(v => new Vector2(v.x, v.z)).ToList();
        var result = GetResult(pto, true);
        var pfrom = result.Select(v => new Vector3(v.x, y, v.y)).Select(v => qfrom * v).ToArray();

        //Get Verticles
        verticles = pfrom;
        triangles = new int[verticles.Length];
        for (int i = 0; i < verticles.Length; i++)
            triangles[i] = i;
        uvcoords = result.ToArray();
    }

    public static List<Vector2> GetResult(List<Vector2> points, bool clockwise)
    {
        var results = new List<Vector2>();
        var lines = points.Select((t, i) => new Line2(t, points[(i + 1) % points.Count])).ToList();

        int j = points.Count;
        while (j-- > 0)
        {
            var a = points[(j + points.Count - 1) % points.Count];
            var b = points[j];
            var c = points[(j + 1) % points.Count];
            var angle = AngleStraight((a - b).normalized, (c - b).normalized);
            var notHavePointsInTriangle = points.All(x => !TriangleStrictlyContains(a, b, c, x));
            var angleCoef = clockwise ? 1 : -1;
            if (angle * angleCoef > 0f && notHavePointsInTriangle && (!Intersect(lines, a, b) && !Intersect(lines, b, c) && !Intersect(lines, a, c)))
            {
                results.AddRange(new[] { a, b, c });
                points.RemoveAt(j);
                j = points.Count;
            }
        }
        return results;
    }

    public static bool HasPointInArea(List<Vector3> areaPoints, Vector3 point, bool yFlat = true)
    {
        int intersectionsCount = 0;
        Line2 line2;
        Vector3 endPoint = point + new Vector3(100000, 0, 0);
        Line2 line1 = new Line2(point, endPoint);

        if (yFlat) for (int i = 0; i < areaPoints.Count; i++)
                areaPoints[i] = new Vector3(areaPoints[i].x, point.y, areaPoints[i].z);

        for (int i = 0; i < areaPoints.Count - 1; i++)
        {
            line2 = new Line2(areaPoints[i], areaPoints[i + 1]);
            if (line1.Intersect(line2))
            {
                Debug.Log($"{areaPoints[i]} {areaPoints[i + 1]} intersect with {point} {endPoint}");
                intersectionsCount++;
            }
        }

        line2 = new Line2(areaPoints[0], areaPoints[areaPoints.Count - 1]);
        if (line1.Intersect(line2))
        {
            Debug.Log($"{areaPoints[0]} {areaPoints.Count - 1} intersect with {point} {endPoint}");
            intersectionsCount++;
        }

        Debug.Log("intersectionsCount " + intersectionsCount);

        if (intersectionsCount % 2 != 1)
            return false;
        else
            return true;
    }

    private static bool Intersect(IEnumerable<Line2> ls, Vector2 start, Vector2 end)
    { return ls.Where(x => !x.CommonFaces(new Line2(start, end))).Any(x => x.Intersect(new Line2(start, end))); }

    private static float AngleStraight(Vector2 v1, Vector2 v2)
    { return Mathf.Atan2(Vector3.Dot(Vector3.forward, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * Mathf.Rad2Deg; }

    private static bool TriangleStrictlyContains(Vector2 a, Vector2 b, Vector2 c, Vector2 point)
    {
        float a1 = (a.x - point.x) * (b.y - a.y) - (b.x - a.x) * (a.y - point.y);
        float b1 = (b.x - point.x) * (c.y - b.y) - (c.x - b.x) * (b.y - point.y);
        float c1 = (c.x - point.x) * (a.y - c.y) - (a.x - c.x) * (c.y - point.y);
        return ((a1 < 0 && b1 < 0 && c1 < 0) || (a1 > 0 && b1 > 0 && c1 > 0));
    }

    private struct Line2
    {
        private readonly Vector2 start, end;

        public Line2(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
        }

        public bool Intersect(Line2 b)
        {
            var n = (b.end - b.start).normalized;
            if (n == (end - b.start).normalized || n == (start - b.end).normalized)
                return true;

            var A1 = end.y - start.y;
            var B1 = start.x - end.x;
            var C1 = -A1 * start.x - B1 * start.y;

            var A2 = b.end.y - b.start.y;
            var B2 = b.start.x - b.end.x;
            var C2 = -A2 * b.start.x - B2 * b.start.y;

            var f1 = A1 * b.start.x + B1 * b.start.y + C1;
            var f2 = A1 * b.end.x + B1 * b.end.y + C1;
            var f3 = A2 * start.x + B2 * start.y + C2;
            var f4 = A2 * end.x + B2 * end.y + C2;

            return f1 * f2 < Mathf.Epsilon && f3 * f4 < Mathf.Epsilon; // строгое пересечение
        }

        public bool CommonFaces(Line2 b)
        { return b.start == start || b.start == end || b.end == start || b.end == end; }
    }
}