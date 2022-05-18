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

    public void CalcAreas()
    {
        List<Vector3> vectors = new List<Vector3>();
        foreach (var item in _corners)
            vectors.Add(item.position);

        Triangulation.GetResult(vectors, true, Vector3.up, out  _vertices, out _triangles, out Vector2[] uv);
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

    public Vector3 GetRandomPointOnMesh()
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

    private void AddDebugPoint()
    {
        _randomPoint = GetRandomPointOnMesh();
        DebugPoints.Add(_randomPoint);
    }

    private void Awake()
    {
        CalcAreas();
        for (int i = 0; i < 1000; i++)
            AddDebugPoint();
    }


    private void OnDrawGizmos()
    {
        if (_corners.Count < 2)
            return;
        Gizmos.color = Color.green;

        for (int i = 0; i < _corners.Count-1; i++)
        {
            Gizmos.DrawLine(_corners[i].position, _corners[i+1].position);
        }

        Gizmos.DrawLine(_corners[0].position, _corners[_corners.Count - 1].position);

        if (DebugPoints.Count <= 0)
            return;
        foreach (Vector3 debugPoint in DebugPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(debugPoint, 1f);
        }
    }
}
