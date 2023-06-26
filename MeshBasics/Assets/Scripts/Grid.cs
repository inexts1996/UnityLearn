using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
    [SerializeField] public int _xSize, _ySize;

    private Vector3[] _vertices;

    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        GenerateMesh();
        GenerateVertices();
        GenerateTriangles();
    }

    private void GenerateTriangles()
    {
        int[] triangles = new int[_xSize * _ySize * 6];

        for (int ti = 0, vi = 0, y = 0; y < _ySize; y++, vi++)
        {
            for (int x = 0; x < _xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + _xSize + 1;
                triangles[ti + 5] = vi + _xSize + 2;
            }
        }

        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
    }

    private Mesh _mesh;

    private void GenerateMesh()
    {
        _mesh = GetComponent<MeshFilter>().mesh = new Mesh();
        _mesh.name = "Procedural Grid";
    }

    private void GenerateVertices()
    {
        _vertices = new Vector3[(_xSize + 1) * (_ySize + 1)];
        Vector2[] uv = new Vector2[_vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        Vector4[] tangents = new Vector4[_vertices.Length];
        for (int i = 0, y = 0; y <= _ySize; y++)
        {
            for (int x = 0; x <= _xSize; x++, i++)
            {
                _vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2(x * 1f / _xSize, y * 1f / _ySize);
                tangents[i] = tangent;
            }
        }

        _mesh.vertices = _vertices;
        _mesh.uv = uv;
        _mesh.tangents = tangents;
    }
}