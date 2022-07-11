using System.Collections.Generic;
using UnityEngine;

public class WalkableArea : MonoBehaviour
{
    [SerializeField] private List<Transform> _corners = new List<Transform>();
    private Vector3 _randomPoint;
    public List<Vector3> DebugPoints;

    private Vector3[] _vertices;
    private int[] _triangles;
    private float[] _sizes;
    private float[] _cumulativeSizes;
    private float _total = 0;

    public static WalkableArea Instance { get; private set; }

    private struct Line
    {
        public Vector3 Start;
        public Vector3 End;

        public Line(Vector3 start, Vector3 end)
        {
            Start = start;
            End = end;
        }
    }

    public void CalcAreas()
    {
        List<Vector3> vectors = new List<Vector3>();
        foreach (var item in _corners)
            vectors.Add(item.position);
        Triangulation.GetResult(vectors, true, Vector3.up, out _vertices, out _triangles, out Vector2[] uv);
        _sizes = GetTriSizes(_triangles, _vertices);
        _cumulativeSizes = new float[_sizes.Length];
        _total = 0;

        for (int i = 0; i < _sizes.Length; i++)
        {
            _total += _sizes[i];
            _cumulativeSizes[i] = _total;
        }
    }

    public float[] GetTriSizes(int[] tris, Vector3[] verts)
    {
        int triCount = tris.Length / 3;
        float[] sizes = new float[triCount];
        for (int i = 0; i < triCount; i++)
        {
            sizes[i] = .5f * Vector3.Cross(
                verts[tris[i * 3 + 1]] - verts[tris[i * 3]],
                verts[tris[i * 3 + 2]] - verts[tris[i * 3]]).magnitude;
        }
        return sizes;
    }

    public bool HasPointInArea(Vector3 point)
    {
        point.y = _vertices[_triangles[0]].y;
        for (int i = 0; i < _triangles.Length / 3; i++)
        {
            Vector3 a = _vertices[_triangles[i * 3]];
            Vector3 b = _vertices[_triangles[i * 3 + 1]];
            Vector3 c = _vertices[_triangles[i * 3 + 2]];
            if (IsinsideTriangle(point, a, b, c))
            {
                Debug.DrawLine(a, b, Color.red);
                Debug.DrawLine(a, c, Color.red);
                Debug.DrawLine(c, b, Color.red);
                return true;
            }
        }
        return false;
    }

    private bool IsinsideTriangle(Vector3 point, Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 pa = a - point;
        Vector3 pb = b - point;
        Vector3 pc = c - point;
        Vector3 pab = Vector3.Cross(pa, pb);
        Vector3 pbc = Vector3.Cross(pb, pc);
        Vector3 pca = Vector3.Cross(pc, pa);

        float d1 = Vector3.Dot(pab, pbc);
        float d2 = Vector3.Dot(pab, pca);
        float d3 = Vector3.Dot(pbc, pca);

        if (d1 > 0 && d2 > 0 && d3 > 0) return true;
        return false;
    }

    public Vector3 GetRandomPointInArea()
    {
        float randomsample = Random.value * _total;
        int triIndex = -1;

        for (int i = 0; i < _sizes.Length; i++)
        {
            if (randomsample <= _cumulativeSizes[i])
            {
                triIndex = i;
                break;
            }
        }

        if (triIndex == -1)
            Debug.LogError("triIndex should never be -1");

        Vector3 a = _vertices[_triangles[triIndex * 3]];
        Vector3 b = _vertices[_triangles[triIndex * 3 + 1]];
        Vector3 c = _vertices[_triangles[triIndex * 3 + 2]];

        // Generate random barycentric coordinates
        float r = Random.value;
        float s = Random.value;

        if (r + s >= 1)
        {
            r = 1 - r;
            s = 1 - s;
        }

        // Turn point back to a Vector3
        Vector3 pointOnArea = a + r * (b - a) + s * (c - a);
        return pointOnArea;
    }

    private void OnEnable()
    {
        Instance = this;
        CalcAreas();
    }

    private void OnDisable() => Instance = null;

    private void OnDrawGizmos()
    {
        if (_corners.Count < 2)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < _corners.Count - 1; i++)
            Gizmos.DrawLine(_corners[i].position, _corners[i + 1].position);

        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;

            for (int i = 0; i < _triangles.Length / 3; i++)
            {
                Vector3 a = _vertices[_triangles[i * 3]];
                Vector3 b = _vertices[_triangles[i * 3 + 1]];
                Vector3 c = _vertices[_triangles[i * 3 + 2]];
                Gizmos.DrawLine(a, b);
                Gizmos.DrawLine(a, c);
                Gizmos.DrawLine(c, b);
            }
        }
    }
}
