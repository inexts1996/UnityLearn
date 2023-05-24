using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] public int _xSize, _ySize, _zSize;

    private Vector3[] _vertices;

    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        GenerateVertices();
        GenerateTriangles();
    }


    private void GenerateTriangles()
    {
        int quads = (_xSize * _ySize + _xSize * _zSize + _ySize * _zSize) * 2;
        var triangles = new int[quads * 6];
        int ring = (_xSize + _zSize) * 2;
        int t = 0, v = 0;
        for (int y = 0; y < _ySize; y++, v++)
        {
            for (int q = 0; q < ring - 1; q++, v++)
            {
                t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
            }

            t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
        }

        t = CreateTopFace(triangles, t, ring);
        _mesh.triangles = triangles;
    }

    private int CreateTopFace(int[] triangles, int t, int ring)
    {
        int tTemp = t;
        int v = _ySize * ring;
        for (int q = 0; q < _xSize - 1; q++, v++)
        {
            tTemp = SetQuad(triangles, tTemp, v, v + 1, v + ring - 1, v + ring);
        }

        tTemp = SetQuad(triangles, tTemp, v, v + 1, v + ring - 1, v + 2);

        return tTemp;
    }

    private static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }

    private Mesh _mesh;

    private void GenerateVertices()
    {
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
        _mesh.name = "Procedural Cube";
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        int cornerVertices = 8;
        int edgeVertices = (_xSize + _ySize + _zSize - 3) * 4;
        int faceVertices = ((_xSize - 1) * (_ySize - 1) + (_xSize - 1) * (_zSize - 1) + (_ySize - 1) * (_zSize - 1)) * 2;

        _vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];

        int v = 0;

        for (int y = 0; y <= _ySize; y++)
        {
            for (int x = 0; x <= _xSize; x++)
            {
                _vertices[v++] = new Vector3(x, y, 0);
            }

            for (int z = 1; z <= _zSize; z++)
            {
                _vertices[v++] = new Vector3(_xSize, y, z);
            }

            for (int x = _xSize - 1; x > -1; x--)
            {
                _vertices[v++] = new Vector3(x, y, _zSize);
            }

            for (int z = _zSize - 1; z > 0; z--)
            {
                _vertices[v++] = new Vector3(0, y, z);
            }
        }

        for (int z = 1; z < _zSize; z++)
        {
            for (int x = 1; x < _xSize; x++)
            {
                _vertices[v++] = new Vector3(x, 0, z);
            }
        }

        for (int z = 1; z < _zSize; z++)
        {
            for (int x = 1; x < _xSize; x++)
            {
                _vertices[v++] = new Vector3(x, _ySize, z);
            }
        }

        _mesh.vertices = _vertices;
    }

    private void OnDrawGizmos()
    {
        if (_vertices == null)
        {
            return;
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.DrawSphere(_vertices[i], 0.1f);
        }
    }
}