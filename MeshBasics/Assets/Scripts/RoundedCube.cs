using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RoundedCube : MonoBehaviour
{
    [SerializeField] private int _xSize, _ySize, _zSize;
    [SerializeField] private int roundness; 

    private Vector3[] _vertices;
    private Vector3[] _normals;

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
        t = CreateBottomFace(triangles, t, ring);
        _mesh.triangles = triangles;
    }

    private int CreateBottomFace(int[] triangles, int t, int ring)
    {
        int v = 1;
        int vMid = _vertices.Length - (_xSize - 1) * (_zSize - 1);
        t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
        for (int x = 1; x < _xSize - 1; x++, v++, vMid++)
        {
            t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
        }

        t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

        int vMin = ring - 2;
        vMid -= _xSize - 2;
        int vMax = v + 2;

        for (int z = 1; z < _zSize - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin, vMid + _xSize - 1, vMin + 1, vMid);
            for (int x = 1; x < _xSize - 1; x++, vMid++)
            {
                t = SetQuad(triangles, t, vMid + _xSize - 1, vMid + _xSize, vMid, vMid + 1);
            }

            t = SetQuad(triangles, t, vMid + _xSize - 1, vMax + 1, vMid, vMax);
        }

        int vTop = vMin - 1;
        t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);

        for (int x = 1; x < _xSize - 1; x++, vTop--, vMid++)
        {
            t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
        }

        t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);
        return t;
    }

    private int CreateTopFace(int[] triangles, int t, int ring)
    {
        int v = ring * _ySize;
        for (int x = 0; x < _xSize - 1; x++, v++)
        {
            t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
        }

        t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);


        int vMin = ring * (_ySize + 1) - 1;
        int vMid = vMin + 1;
        int vMax = v + 2;
        for (int z = 1; z < _zSize - 1; vMin--, vMid++, vMax++, z++)
        {
            t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + _xSize - 1);

            for (int x = 1; x < _xSize - 1; x++, vMid++)
            {
                t = SetQuad(triangles, t, vMid, vMid + 1, vMid + _xSize - 1, vMid + _xSize);
            }

            t = SetQuad(triangles, t, vMid, vMax, vMid + _xSize - 1, vMax + 1);
        }

        int vTop = vMin - 2;
        t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
        for (int x = 1; x < _xSize - 1; x++, vTop--, vMid++)
        {
            t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
        }

        t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);

        return t;
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
        _normals = new Vector3[_vertices.Length];

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
                _vertices[v++] = new Vector3(x, _ySize, z);
            }
        }

        for (int z = 1; z < _zSize; z++)
        {
            for (int x = 1; x < _xSize; x++)
            {
                _vertices[v++] = new Vector3(x, 0, z);
            }
        }

        _mesh.normals = _normals;
        _mesh.vertices = _vertices;
    }

    private void OnDrawGizmos()
    {
        if (_vertices == null)
        {
            return;
        }

        for (int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(_vertices[i], 0.1f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(_vertices[i], _normals[i]);
        }
    }
}
