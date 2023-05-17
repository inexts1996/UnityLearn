using UnityEngine;
using System.Collections;

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
        int[] triangles = new int[3];
        triangles[0] = 0;
        triangles[1] = _xSize + 1;
        triangles[2] = 1;
        _mesh.triangles = triangles;
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
        for (int i = 0, y = 0; y <= _ySize; y++)
        {
            for (int x = 0; x <= _xSize; x++, i++)
            {
                _vertices[i] = new Vector3(x, y);
            }
        }

        _mesh.vertices = _vertices;
    }

    private void OnDrawGizmos()
    {
        if (null == _vertices) return;
        Gizmos.color = Color.black;
        foreach (var pos in _vertices)
        {
            Gizmos.DrawSphere(pos, 0.1f);
        }
    }
}